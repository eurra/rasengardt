// Capa de las Sombras.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{

	public class CapaSombras : BaseCloak , SkillArtifact
	{
		private bool m_OnUse = false;
		private InternalTimer timer;
		
		public static TimeSpan delay = TimeSpan.FromMinutes( 5 );
		
		public bool OnUse{ get{ return m_OnUse; } }

		public override int PhysicalResistance{ get{ return 5; } }
		public override int ColdResistance{ get{ return 5; } }
		
		private static int HidingBonus = 20;
                private static int StealthBonus = 10;
		private SkillMod smod1 = new DefaultSkillMod( SkillName.Hiding, true, HidingBonus );
                private SkillMod smod2 = new DefaultSkillMod( SkillName.Stealth, true, StealthBonus );

/*************************************************/
/******* CONSTRUCTORES Y METODOS ASOCIADOS *******/
/*************************************************/

		private Type m_TipoBase;

		[CommandProperty( AccessLevel.GameMaster )]
		public Type TipoBase
  		{
  			get{ return m_TipoBase; }

  			set
  			{
    				if( value == null || !SAUtility.EsTipoBaseValido( this.GetType(), value ) )
                        		m_TipoBase = typeof(Cloak);
				else
                        		m_TipoBase = value;

				ConfigurarTipoBase();
				InvalidateProperties();
			}
		}

		private void ConfigurarTipoBase()
		{
			Item cloak = Activator.CreateInstance( m_TipoBase ) as Item;

			if( cloak == null )
				return;

  			ItemID = cloak.ItemID;

			Weight = cloak.Weight;
			Layer = cloak.Layer;

			cloak.Delete();
		}
		
		[Constructable]
		public CapaSombras() : this( SAUtility.TipoBaseRandom( typeof(CapaSombras) ) )
		{
		}

		[Constructable]
		public CapaSombras( Type tipocapa ) : base( 0x13B )
		{
			TipoBase = tipocapa;
			SetProps( null );
  		}

		public CapaSombras( BaseCloak CapaBase ) : base( 0x13B )
		{
			Type tipo = null;
			
			if( CapaBase != null )
				tipo = CapaBase.GetType();

			TipoBase = tipo;
			SetProps( CapaBase as BaseClothing );
		}

		public CapaSombras( Serial serial ) : base( serial )
		{
		}

 		private void SetProps( BaseClothing RopaBase )
		{
			if( RopaBase != null )
			{
				SAUtility.CopyAosAttributes( RopaBase, this as BaseClothing );
			}

                	Hue = 193;
			Name = SAUtility.NombreArtefacto( GetType() );
		}

/**********************************/
/******* HABILIDAD PRIMARIA *******/
/**********************************/

		public override void OnDoubleClick( Mobile from )
		{
			if ( !(from is dovPlayerMobile) )
				return;

			if ( Parent != from )
                        {
                        	from.SendMessage("Debes tener equipada la Capa para usarla.");
                        	return;
                        }
                        
                        if ( !SkillRequerida(from) )
                        {
                        	from.SendMessage("Esta acción requiere que tengas 80 de Hiding y 40 de Stealth minimo.");
                        	return;
                        }

			if( ((dovPlayerMobile)from).CSLastDobleClick + delay > DateTime.Now )
			{
                        	from.SendMessage("Debes esperar 5 minutos antes de volver a usar la Capa.");
                        	return;
                        }
                        
                        if( from.Hidden )
			{
                        	from.SendMessage("No puedes usar la Capa ya oculto.");
                        	return;
                        }
                        
                        foreach ( Mobile check in from.GetMobilesInRange( 2 ) )
			{
				if ( check.InLOS( from ) && check.Combatant == from )
				{
                                	from.SendMessage("Debes estar a 2 pasos de cualquier enemigo para que la capa funcione.");
                        		return;
				}
			}
                        
                        SetInvis( from );
		}
		
		private void SetInvis( Mobile from )
		{
			((dovPlayerMobile)from).CSLastDobleClick = DateTime.Now;
			timer = new InternalTimer( from, this );
			m_OnUse = true;

			from.PlaySound( 515 );
                        from.FixedParticles( 0x376A, 1, 25, 9910, 193, 0, EffectLayer.Waist );

			from.Warmode = false;
                	timer.Start();

                	from.Hidden = true;
                	from.LocalOverheadMessage( MessageType.Regular, 193, true, "Has logrado esconderte en las sombras...");
  		}
		
		public bool SkillRequerida( Mobile m )
		{
			if ( m.Skills.Hiding.Value < 80.0 || m.Skills.Stealth.Value < 40.0 )
				return false;
			
			return true;
		}
		
		private void DesactivarInvis( Mobile m )
                {
			if( timer != null )
			{
				timer.Stop();
                		timer = null;
   			}
			m_OnUse = false;
			m.RevealingAction();
			m.LocalOverheadMessage( MessageType.Regular, 193, true, "Has sido revelado!");
                }

                private class InternalTimer : Timer
		{
			private Mobile m_From; 
			private CapaSombras Cloak;
			private int counter;

			public InternalTimer( Mobile from, CapaSombras cloak ) : base ( TimeSpan.Zero, TimeSpan.FromSeconds(250/1000) )
			{
				Priority = TimerPriority.TwoFiftyMS;
				m_From = from;
				Cloak = cloak;
				counter = 0;
			}

			protected override void OnTick()
			{
				counter++;

				if ( !m_From.Hidden || counter >= 40 )
				{
					Cloak.DesactivarInvis( m_From );
                                        return;
                                }
			}

		}

/*****************************************/
/******* OTROS METODOS ADICIONALES *******/
/*****************************************/

                public void Switch( Mobile m )
                {
                	m.SendMessage( "Este artefacto no puede cambiar!" );
                }
		
		public override bool Dye( Mobile from, DyeTub sender )
  		{
                	from.SendMessage("No puedes hacer eso en este item!");
			return false;
  		} 

		public override void OnRemoved(object parent)
		{
			if ( !(parent is Mobile) )
				return;

			if( smod1 != null )
				smod1.Remove();
			if( smod2 != null )
				smod2.Remove();

                        if ( m_OnUse )
				DesactivarInvis( (Mobile)parent );

                        base.OnRemoved( parent );
                }

                private void AddSkillMods( Mobile m )
                {
                	if( m == null )
                		return;
                	
                	m.AddSkillMod( smod1 );
                        m.AddSkillMod( smod2 );
                }

                public override bool OnEquip( Mobile from )
		{
                       	if( SAUtility.MuchosEquipados( from ) )
                        {
                        	from.SendMessage( "No puedes equiparte otro artefacto." );
                        	return false;
                        }

			AddSkillMods( from );

                        return base.OnEquip( from );
                }
                
 		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060660, "Bonus Hiding\t" + HidingBonus.ToString() + "\nBonus Stealth: " + StealthBonus.ToString() );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			
			writer.Write( m_TipoBase.Name );

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			TipoBase = ScriptCompiler.FindTypeByName( reader.ReadString() );

			AddSkillMods( Parent as Mobile );
		}

	}
	
}
