// Este script esta hecho para cambiar de forma generica algunos npcs que puedan sufrir cambios.
// NPC actual = Hiryu.

using System;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
 	public class ConvertirNPC
 	{
 		public static void Initialize()
		{
			Server.Commands.Register( "ConvertirNPC", AccessLevel.Administrator, new CommandEventHandler( ConvertirNPC_OnCommand ) );
		}

       		[Usage( "ConvertirNPC" )]
		[Description( "Otorga ciertas props a todos los npcs en el mundo de un tito dado." )]
		private static void ConvertirNPC_OnCommand( CommandEventArgs e )
		{
			int count = 0;

			foreach( Mobile m in World.Mobiles.Values )
			{
				if( m is ZorstrickFire )
				{
					BaseCreature c = m as BaseCreature;

					c.SetStr( 180, 220 );
					c.SetDex( 150, 180 );
					c.SetInt( 80, 120 );

					c.SetHits( 330, 370 );

					c.SetDamage( 20, 25 );

					c.SetDamageType( ResistanceType.Physical, 30  );
					c.SetDamageType( ResistanceType.Cold, 10 );
					c.SetDamageType( ResistanceType.Fire, 90 );
					c.SetDamageType( ResistanceType.Energy, 10 );
					c.SetDamageType( ResistanceType.Poison, 10 );

					c.SetResistance( ResistanceType.Physical, 50 );
					c.SetResistance( ResistanceType.Fire, 100 );
					c.SetResistance( ResistanceType.Cold, 20 );
					c.SetResistance( ResistanceType.Poison, 40 );
					c.SetResistance( ResistanceType.Energy, 30 );

					c.SetSkill( SkillName.MagicResist, 60.0, 70.0 );
					c.SetSkill( SkillName.Anatomy, 70.1, 80.0 );
					c.SetSkill( SkillName.Tactics, 90.1, 100.1 );
					c.SetSkill( SkillName.Wrestling, 80.1, 90.1 );

					c.Fame = 5000;
					c.Karma = 0;

					c.VirtualArmor = 30;

					c.Tamable = true;
					c.ControlSlots = 2;
					c.MinTameSkill = 85.1;
					
					count++;
				}
				else if ( m is ZorstrickIce )
				{
					BaseCreature c = m as BaseCreature;

					c.SetStr( 180, 220 );
					c.SetDex( 150, 180 );
					c.SetInt( 80, 120 );

					c.SetHits( 330, 370 );

					c.SetDamage( 20, 25 );

					c.SetDamageType( ResistanceType.Physical, 30 );
					c.SetDamageType( ResistanceType.Cold, 90 );
					c.SetDamageType( ResistanceType.Fire, 10 );
					c.SetDamageType( ResistanceType.Energy, 10 );
					c.SetDamageType( ResistanceType.Poison, 10 );

					c.SetResistance( ResistanceType.Physical, 50 );
					c.SetResistance( ResistanceType.Fire, 10 );
					c.SetResistance( ResistanceType.Cold, 100 );
					c.SetResistance( ResistanceType.Poison, 40 );
					c.SetResistance( ResistanceType.Energy, 30 );

					c.SetSkill( SkillName.MagicResist, 60.0, 70.0 );
					c.SetSkill( SkillName.Anatomy, 70.1, 80.0 );
					c.SetSkill( SkillName.Tactics, 90.1, 100.1 );
					c.SetSkill( SkillName.Wrestling, 80.1, 90.1 );

					c.Fame = 5000;
					c.Karma = 0;

					c.VirtualArmor = 30;

					c.Tamable = true;
					c.ControlSlots = 2;
					c.MinTameSkill = 85.1;
					
					count++;
				}
			}

			e.Mobile.SendMessage("Zorstricks Convertidos: {0}", count);
		}
	 }
}
