SUBDIRS = . Autotools

include $(top_srcdir)/xbuild.include

PARSE = parse.exe

parse: $(PARSE)

$(PARSE): parse.cs all
	$(CSC) -out:$@ parse.cs $(REFS) -r:$(ASSEMBLY)

CLEANFILES = $(PARSE) $(PARSE).mdb
