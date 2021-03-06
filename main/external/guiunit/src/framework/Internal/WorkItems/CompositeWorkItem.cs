// ***********************************************************************
// Copyright (c) 2012 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Threading;
using NUnit.Framework.Internal.Commands;
using NUnit.Framework.Api;

namespace NUnit.Framework.Internal.WorkItems
{
    /// <summary>
    /// A CompositeWorkItem represents a test suite and
    /// encapsulates the execution of the suite as well
    /// as all its child tests.
    /// </summary>
    public class CompositeWorkItem : WorkItem
    {
        private TestSuite _suite;
        private ITestFilter _childFilter;
#if CLR_2_0 || CLR_4_0
        private System.Collections.Generic.Queue<WorkItem> _children = new System.Collections.Generic.Queue<WorkItem>();
#else
        private System.Collections.Queue _children = new System.Collections.Queue();
#endif
        private TestCommand _setupCommand;
        private TestCommand _teardownCommand;

        private CountdownEvent _childTestCountdown;

        /// <summary>
        /// Construct a CompositeWorkItem for executing a test suite
        /// using a filter to select child tests.
        /// </summary>
        /// <param name="suite">The TestSuite to be executed</param>
        /// <param name="context">The execution context in which to run the suite</param>
        /// <param name="childFilter">A filter used to select child tests</param>
        public CompositeWorkItem(TestSuite suite, TestExecutionContext context, ITestFilter childFilter)
            : base(suite, context)
        {
            _suite = suite;
            _setupCommand = suite.GetOneTimeSetUpCommand();
            _teardownCommand = suite.GetOneTimeTearDownCommand();
            _childFilter = childFilter;
        }

        /// <summary>
        /// Method that actually performs the work. Overridden
        /// in CompositeWorkItem to do setup, run all child
        /// items and then do teardown.
        /// </summary>
        protected override void PerformWork()
        {
            // Assume success, since the result will be inconclusive
            // if there is no setup method to run or if the
            // context initialization fails.
            Result.SetResult(ResultState.Success);

            PerformOneTimeSetUp();

            if (Result.ResultState.Status == TestStatus.Passed && _suite.HasChildren)
            {
                foreach (Test test in _suite.Tests)
                    if (_childFilter.Pass(test))
                        _children.Enqueue(CreateWorkItem(test, this.Context, _childFilter));

                if (_children.Count > 0)
                {
                    RunChildren();
                    return;
                }
                else
                {
                    Console.WriteLine("No tests found that match filter: " + _childFilter.ToString() + "\n");
                    Result.SetResult(ResultState.Inconclusive);
                }
            }

            // Fall through in case there were no child tests to run.
            // Otherwise, this is done in the completion event.
            PerformOneTimeTearDown();

            WorkItemComplete();
        }

        #region Helper Methods

        private void PerformOneTimeSetUp()
        {
            try
            {
                _setupCommand.Execute(Context);

                // SetUp may have changed some things
                Context.UpdateContext();
            }
            catch (Exception ex)
            {
                if (ex is NUnitException || ex is System.Reflection.TargetInvocationException)
                    ex = ex.InnerException;

                Result.RecordException(ex, FailureSite.SetUp);
            }
        }

        private void RunChildren()
        {
            _childTestCountdown = new CountdownEvent(_children.Count);

            while (_children.Count > 0)
            {
                WorkItem child = (WorkItem)_children.Dequeue();
                child.Completed += new EventHandler(OnChildCompleted);
                child.Execute();
            }
        }

        private void PerformOneTimeTearDown()
        {
            TestExecutionContext.SetCurrentContext(Context);
            _teardownCommand.Execute(Context);
        }

        private object _completionLock = new object();

        private void OnChildCompleted(object sender, EventArgs e)
        {
            lock (_completionLock)
            {
                WorkItem childTask = sender as WorkItem;
                if (childTask != null)
                {
                    childTask.Completed -= new EventHandler(OnChildCompleted);
                    Result.AddResult(childTask.Result);
                    _childTestCountdown.Signal();

                    if (_childTestCountdown.CurrentCount == 0)
                    {
                        PerformOneTimeTearDown();
                        WorkItemComplete();
                    }
                }
            }
        }

        #endregion
    }
}
