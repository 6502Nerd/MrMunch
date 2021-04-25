using System.Windows.Forms;
using System.IO;
using SharpDX.DirectSound;


/// <summary>
/// Summary description for sound
/// </summary>
public class DDSound
{
    public static DirectSound ds;
    public static Control dsOwner;

	public static bool music = false;					        // Whether music (looped sounds) is to be played
	public static bool effects = true;					        // Whether effects (non-looped sounds) are to be played

	public static SecondarySoundBuffer dsbMainTune;				// Handle of main tune
	public static SecondarySoundBuffer dsbEatDot;				// Handle of eat dot sound
	public static SecondarySoundBuffer dsbPowerMunch;			// Handle of power munch sound
	public static SecondarySoundBuffer dsbEatGhost;				// Handle of ghost eaten sound
	public static SecondarySoundBuffer dsbEatPowerPill;			// Handle of eat power pill sound
	public static SecondarySoundBuffer dsbLevelComplete;		// Handle of level complete sound
	public static SecondarySoundBuffer dsbEatFruit;				// Handle of fruit eaten sound
	public static SecondarySoundBuffer dsbGot3Fruit;			// Handle of 3 fruits got sound
	public static SecondarySoundBuffer dsbMunchDead;			// Handle of munch dead sound

	public DDSound(Control owner)
	{
        dsOwner = owner;
        ds = new DirectSound();
        ds.SetCooperativeLevel(dsOwner.Handle, CooperativeLevel.Priority);

        if (owner != null)
    		LoadAllSounds();
	}

    SecondarySoundBuffer LoadHelper(Stream ioStream)
    {
        SecondarySoundBuffer dsbBuffer;
        SoundBufferDescription dsDesc;
        SharpDX.DataStream sdxStream1, sdxStream2;
        BinaryReader br = new BinaryReader(ioStream);

        br.ReadInt32();
        br.ReadInt32();
        br.ReadInt32();

        br.ReadInt32();
        br.ReadInt32();
        br.ReadInt16();
        int numChannels = br.ReadInt16();
        int sampleRate = br.ReadInt32();
        br.ReadInt32();
        br.ReadInt16();
        int bitsPerSample = br.ReadInt16();
        br.ReadInt32();
        int dataLength = br.ReadInt32();
        
        dsDesc = new SoundBufferDescription();
        dsDesc.Flags = BufferFlags.None;
        dsDesc.BufferBytes = dataLength;
        dsDesc.Format = new SharpDX.Multimedia.WaveFormat(sampleRate, bitsPerSample, numChannels);
        dsbBuffer = new SecondarySoundBuffer(ds, dsDesc);

        sdxStream1 = dsbBuffer.Lock(0, dsDesc.BufferBytes, LockFlags.EntireBuffer, out sdxStream2);
        byte []tempBuf = new byte[dataLength];
        ioStream.Read(tempBuf, 0, dataLength);
        sdxStream1.Write(tempBuf, 0, dataLength);
        dsbBuffer.Unlock(sdxStream1, sdxStream2);

        return dsbBuffer;
    }

    public void LoadAllSounds()
	{
        dsbMainTune = LoadHelper(MrMunch.Properties.Resources.mazetoon);
        dsbEatDot = LoadHelper(MrMunch.Properties.Resources.eatdot);
        dsbPowerMunch = LoadHelper(MrMunch.Properties.Resources.clocktickfast);
        dsbEatGhost = LoadHelper(MrMunch.Properties.Resources.eatghost);
        dsbEatPowerPill = LoadHelper(MrMunch.Properties.Resources.eatpill);
        dsbLevelComplete = LoadHelper(MrMunch.Properties.Resources.ar2);
        dsbEatFruit = LoadHelper(MrMunch.Properties.Resources.eatfruit);
        dsbGot3Fruit = LoadHelper(MrMunch.Properties.Resources.ar2);
        dsbMunchDead = LoadHelper(MrMunch.Properties.Resources.scream);
	}

	public static void StopAllLoopSounds()
	{
		dsbMainTune.Stop();
	}

	public static void StopAllFxSounds()
	{
		dsbPowerMunch.Stop();
		dsbEatPowerPill.Stop();
		dsbEatDot.Stop();
		dsbEatGhost.Stop();
		dsbLevelComplete.Stop();
		dsbEatFruit.Stop();
		dsbGot3Fruit.Stop();
	}

	public static void StopAllSounds()
	{
		StopAllLoopSounds();
		StopAllFxSounds();
	}

	public static void PlayOnce(SecondarySoundBuffer dsbBuffer)
	{
		if(effects)
		{
			dsbBuffer.Play(0, PlayFlags.None);
		}
	}

	public static void PlayLoop(SecondarySoundBuffer dsbBuffer)
	{
		if(music)
		{
            dsbBuffer.Play(0, PlayFlags.Looping);
		}
	}

	public static void Stop(SecondarySoundBuffer dsbBuffer)
	{
		dsbBuffer.Stop();
	}
}
