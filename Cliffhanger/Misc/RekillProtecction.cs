using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Misc
{
	/*public class RekillProtInfo
	{
		private DateTime m_ResDate;
		
		public DateTime ResDate{ get{ return m_Date; } }

		public RekillProtInfo( DateTime resdate )
		{
			m_ResDate = resdate;
		}
	} */

	public class RekillProt
	{
        	private static Hashtable m_List = new Hashtable();
        	private static readonly TimeSpan ProtDuration = TimeSpan.FromMinutes( 2.5 );
        	private const int DistToCorpse = 5;
        	private const int DistToRes = 10;
        	
        	public static void AddInfo( PlayerMobile victim )
		{
			m_List[victim] = DateTime.Now;
			victim.SendMessage( "Protección contra re-kills activada." );
		}

		public static bool Contains( PlayerMobile victim )
		{
                	Clean();

			return ( m_List[victim] == null ? false : true );
		}

		public static void RemoveInfo( PlayerMobile victim )
		{
			m_List.Remove( victim );
			victim.SendMessage( "Protección contra re-kills anulada." );
		}

		private static bool CheckPlayer( Mobile m )
		{
			if( m == null )
				return false;

			if( m.Player )
				return true;
			else if( !( m is BaseCreature ) )
				return false;
				
			BaseCreature c = (BaseCreature)m;

			if( ( c.ControlMaster != null && c.ControlMaster.Player ) || ( c.SummonMaster != null && c.SummonMaster.Player ) )
				return true;
			else
				return false;
		}
		
		private static void Clean()
		{
			ArrayList rem = new ArrayList();

			foreach( Mobile m in m_List.Keys )
			{
                                DateTime date = (DateTime)m_List[m];

				if( date + ProtDuration < DateTime.Now )
                                	rem.Add( m );
			}
			
			foreach( Mobile m in rem )
				m_List.Remove( m );
		}
		
		private static bool AllowKill( Mobile victim )
		{
			foreach( Mobile m in victim.GetMobilesInRange( DistToRes ) )
			{
				if( m is Healer )
					return false;
			}

			foreach( Item i in victim.GetItemsInRange( DistToRes ) )
			{
				if( i is AnkhWest || i is AnkhEast )
					return false;

				if( i is Corpse && ((Corpse)i).Owner == victim && victim.InRange( i.Location, DistToCorpse ) )
					return true;
			}
			
			return false;
		}

		public static bool IsProtected( Mobile victim, Mobile killer )
		{
			if( ( victim == null ) || ( killer == null ) || !victim.Player )
				return false;

			return ( RekillProt.CheckPlayer( killer ) && RekillProt.Contains( victim as PlayerMobile ) && !RekillProt.AllowKill( victim ) );
		}
	}
}
