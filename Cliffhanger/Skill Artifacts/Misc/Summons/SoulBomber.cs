// SoulBomber.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class SoulBomber : BaseCreature
	{
		public override bool DeleteCorpseOnDeath{ get{ return true; } }

		[Constructable]
		public SoulBomber( int body ) : base( AIType.AI_Melee, FightMode.None, 15, 1, 0.0, 0.0 )
		{
			Name = "SoulBomber";
			Body = body;

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 10 );

			SetHits( 5 );

			SetDamage( 0 );

			SetSkill( SkillName.Wrestling, 120.0 );

			ControlSlots = 0;

		}

		public SoulBomber( Serial serial ) : base( serial )
		{
		}

		public override bool OnBeforeDeath()
		{
                	Explode( Location, Map, Hue );
                	
                	return base.OnBeforeDeath();
                }

		private static void Explode( Point3D loc, Map map, int Hue )
		{
			Effects.SendLocationEffect( loc, map, 0x36B0, 15, 9961, 0 );
			
			ArrayList arr = new ArrayList();

			foreach ( Mobile m in map.GetMobilesInRange( loc, 3 ) )
			{
				if( m != null && !( m is SoulBomber ) )
					arr.Add( m );
			}

                        foreach ( Mobile m in arr )
			{
				if( m != null && m.Alive )
				{
					Effects.SendLocationEffect( new Point3D( m.X, m.Y, m.Z ), m.Map, 0x36B0, 15, Hue, 0 );
					Effects.SendLocationEffect( new Point3D( m.X, m.Y, m.Z + 1), m.Map, 0x36BD, 15, 0, 0 );
            				Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z ), m.Map, 0x36CB, 15, 0, 0 );
            				Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z ), m.Map, 0x36BD, 15, 0, 0 );
            				Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z ), m.Map, 0x36CB, 15, 0, 0 );

                               		Effects.PlaySound ( new Point3D( m.X, m.Y, m.Z ), m.Map, 0x307 );

					if( m.InRange( loc, 1 ) )
                                		m.Damage( Utility.RandomMinMax(23,25), null );
					else if( m.InRange( loc, 2 ) )
                                        	m.Damage( Utility.RandomMinMax(15,18), null );
                                	else if( m.InRange( loc, 3 ) )
                                        	m.Damage( Utility.RandomMinMax(5,8), null );
                                }
                        }
		}

		public override void OnThink()
		{
			base.OnThink();
			
                        if( m_OnExplode && TargetLocation == Point2D.Zero )
                        {
                        	if( Combatant == null || !Combatant.Alive || InRange( Combatant.Location, 1 ) )
					Kill();
			}
		}
		
		public override void OnCombatantChange()
		{
                	base.OnCombatantChange();
                	
                        if( Combatant != null )
				TargetBomb( new Point2D( Combatant.X, Combatant.Y ) );
		}

		protected override bool OnMove( Direction d )
		{
			Effects.SendLocationEffect( new Point3D( X, Y, Z ), Map, 0x36CB, 15, Hue, 0 );

			return base.OnMove( d );
		}
		
		private bool m_OnExplode;

		public void TargetBomb( Point2D p )
		{
			m_OnExplode = true;

                	TargetLocation = p;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
