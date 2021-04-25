using System;

namespace MrMunch
{
	public struct PcTableEntry
	{
		public int retVal;				// The value to return.
		public int chance;				// The chance of the value being returned.

		public PcTableEntry(int r, int c)
		{
			retVal = r;
			chance = c;
		}
	};

	/// <summary>
	/// Summary description for MunchRandom.
	/// </summary>
	public class MunchRandom : Random
	{
		public MunchRandom()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public bool PcCheck(int val)
		{
			if((int)(Sample() * 1000) < val)
				return(true);
			else
				return(false);
		}


		public int PcTable(PcTableEntry[] pcTable)
		{
			int i, chance;
			int totChance;

			i = 0;
			totChance = 0;
			chance = (int)(Sample() * 1000) ;
			do
			{
				totChance += pcTable[i].chance;
				i++;
			}
			while(totChance < chance);
			i--;
			return(pcTable[i].retVal);
		}
	}
}
