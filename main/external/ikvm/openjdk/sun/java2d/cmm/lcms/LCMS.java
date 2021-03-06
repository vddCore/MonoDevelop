/*
  Copyright (C) 2009 Jeroen Frijters
  Copyright (C) 2010 Volker Berlin (i-net software)

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
package sun.java2d.cmm.lcms;

import gnu.java.awt.color.TagEntry;

import java.awt.color.CMMException;
import java.awt.color.ColorSpace;
import java.awt.color.ICC_Profile;
import java.awt.color.ICC_ProfileGray;
import java.awt.color.ICC_ProfileRGB;
import java.awt.image.BufferedImage;
import java.awt.image.Raster;
import java.awt.image.WritableRaster;
import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Hashtable;

import sun.java2d.cmm.ColorTransform;
import sun.java2d.cmm.PCMM;

// dummy color management implementation
public class LCMS implements PCMM {
    
    private static final int HEADER_SIZE = 128;
    
    private final ArrayList<ProfileData> profiles = new ArrayList<ProfileData>();

    public synchronized long loadProfile( byte[] data ) {
        int free = profiles.indexOf( null );
        if( free != -1 ) {
            profiles.set( free, new ProfileData( data.clone()) );
            return free;
        } else {
            long id = profiles.size();
            profiles.add( new ProfileData( data.clone()) );
            return id;
        }
    }

    public synchronized void freeProfile( long profileID ) {
        profiles.set( (int)profileID, null );
    }

    public synchronized int getProfileSize( long profileID ) {
        return profiles.get( (int)profileID ).data.length;
    }

    public synchronized void getProfileData( long profileID, byte[] data ) {
        byte[] src = profiles.get( (int)profileID ).data;
        System.arraycopy( src, 0, data, 0, src.length );
    }

    public void getTagData( long profileID, int tagSignature, byte[] data ) {
        ProfileData profile = profiles.get( (int)profileID );
        if( tagSignature == ICC_Profile.icSigHead ) {
            byte[] src = profile.data;
            System.arraycopy( src, 0, data, 0, HEADER_SIZE );
        } else {
            TagEntry entry = profile.tags.get( tagSignature );
            if( entry == null ){
                throw new CMMException( "tag does not exist: " + tagSignature );
            }
            byte[] src = entry.getData();
            System.arraycopy( src, 0, data, 0, src.length );
        }
        
    }

    public int getTagSize( long profileID, int tagSignature ) {
        if( tagSignature == ICC_Profile.icSigHead ) {
            return HEADER_SIZE;
        }
        ProfileData profile = profiles.get( (int)profileID );
        TagEntry entry = profile.tags.get( tagSignature );
        if( entry == null ){
            throw new CMMException( "tag does not exist: " + tagSignature );
        }
        return entry.getData().length;
    }

    public void setTagData(long profileID, int tagSignature, byte[] data)
    {
        throw new CMMException("Not implemented");
    }

    public ColorTransform createTransform(ICC_Profile profile, int renderType, int transformType)
    {
        return new DummyColorTransform();
    }

    public ColorTransform createTransform(ColorTransform[] transforms)
    {
        return new DummyColorTransform();
    }
    
    private static class ProfileData{
        
        private final byte[] data;
        private final Hashtable<Integer, TagEntry> tags;
        
        private ProfileData(byte[] data){
            this.data = data;
            this.tags = createTagTable( data );
        }
        
        private static Hashtable<Integer, TagEntry> createTagTable( byte[] data ) throws IllegalArgumentException {
            ByteBuffer buf = ByteBuffer.wrap( data );
            int nTags = buf.getInt( HEADER_SIZE );

            Hashtable<Integer, TagEntry> tagTable = new Hashtable<Integer, TagEntry>();
            for( int i = 0; i < nTags; i++ ) {
                int sig = buf.getInt( HEADER_SIZE + i * TagEntry.entrySize + 4 );
                int offset = buf.getInt( HEADER_SIZE + i * TagEntry.entrySize + 8 );
                int size = buf.getInt( HEADER_SIZE + i * TagEntry.entrySize + 12 );
                TagEntry te = new TagEntry( sig, offset, size, data );

                if( tagTable.put( sig, te ) != null )
                    throw new IllegalArgumentException( "Duplicate tag in profile:" + te );
            }
            return tagTable;
        }

    }
    
    static class DummyColorTransform implements ColorTransform
    {
        public int getNumInComponents()
        {
            throw new CMMException("Not implemented");
        }

        public int getNumOutComponents()
        {
            throw new CMMException("Not implemented");
        }

        public void colorConvert(BufferedImage src, BufferedImage dst)
        {
            throw new CMMException("Not implemented");
        }

        public void colorConvert(Raster src, WritableRaster dst, float[] srcMinVal, float[] srcMaxVal, float[] dstMinVal, float[]dstMaxVal)
        {
            throw new CMMException("Not implemented");
        }

        public void colorConvert(Raster src, WritableRaster dst)
        {
            throw new CMMException("Not implemented");
        }

        public short[] colorConvert(short[] src, short[] dest)
        {
            throw new CMMException("Not implemented");
        }

        public byte[] colorConvert(byte[] src, byte[] dest)
        {
            throw new CMMException("Not implemented");
        }
    }
}
