// Piedra de Karma-Fama-Kills
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using System.Collections;

namespace Server.Items
{

	public class KFKStone : Item
  	{
                private ArrayList Almacenados;

		[Constructable]
		public KFKStone() : base( 0xED4 )
		{
                        Name = "Piedra de Karma/Fama/Kills";
                        Hue = 2425;
			Movable = false;
			Almacenados = new ArrayList();
 	     	}

     		public KFKStone( Serial serial ) : base( serial )
      		{
      		}

 	     	public override void OnDoubleClick( Mobile from )
 	     	{

                	if( !Ingresado( from ) )
                	{
                		from.SendGump( new KFKGumpI( from, this ) );
                	}
                	else
                	{
                                from.SendGump( new KFKGumpS( from, this ) );
                 	}

		}

		public void IngresarPlayer( Mobile m )
		{
			
			Almacenador reg = new Almacenador( m, m.Karma, m.Fame, m.Kills );
                        Almacenados.Add( reg );
			
		}
		
		public void SacarPlayer( Mobile m )
		{
			
			for ( int i = 0; i < Almacenados.Count; ++i )
			{
				Almacenador temp = (Almacenador)Almacenados[i];

				if( m == temp.m_From )
				{
					temp.m_From.Karma = temp.m_Karma;
					temp.m_From.Fame = temp.m_Fama;
					temp.m_From.Kills = temp.m_Kills;
					Almacenados.Remove( Almacenados[i] );
					return;
				}
			}
			
		}
		
		private bool Ingresado( Mobile m )
		{

			for ( int i = 0; i < Almacenados.Count; ++i )
			{
                                Almacenador temp = (Almacenador)Almacenados[i];
				if( m == temp.m_From )
					return true;
			}
			
			return false;
		}
		
		private void VaciarPiedra()
		{
			for ( int i = 0; i < Almacenados.Count; ++i )
			{
				Almacenador temp = (Almacenador)Almacenados[i];

				if( temp.m_From != null )
				{
                                	temp.m_From.Karma = temp.m_Karma;
					temp.m_From.Fame = temp.m_Fama;
					temp.m_From.Kills = temp.m_Kills;
				}
                                Almacenados.Remove( Almacenados[i] );
			}
			
		}

		public override void OnAfterDelete()
		{

			VaciarPiedra();
			base.OnAfterDelete();

         	} 

      		public override void Serialize( GenericWriter writer )
      		{
        		base.Serialize( writer );

        		writer.Write( (int) 0 ); // version

                        writer.Write( Almacenados.Count );
        		for( int i = 0; i < Almacenados.Count; i++ )
			{
				Almacenador temp = (Almacenador)Almacenados[i];
				writer.Write( temp.m_From );
				writer.Write( temp.m_Karma );
				writer.Write( temp.m_Fama );
				writer.Write( temp.m_Kills );
			}

        	}

      		public override void Deserialize( GenericReader reader )
      		{
        		base.Deserialize( reader );

         		int version = reader.ReadInt();

         		Almacenados = new ArrayList();
         		int count = reader.ReadInt();
                        for( int i = 0; i < count; i++ )
			{
				Almacenador temp = new Almacenador( reader.ReadMobile(), reader.ReadInt(), reader.ReadInt(), reader.ReadInt() );
                                Almacenados.Add( temp );
			}

      		}
      		
      		private class Almacenador
		{
			public Mobile m_From;
			public int m_Karma;
			public int m_Fama;
			public int m_Kills;

			public Almacenador( Mobile from, int karma, int fama, int kills )
			{
				m_From = from;
				m_Karma = karma;
				m_Fama = fama;
				m_Kills = kills;
			}
		}
   	}
}
