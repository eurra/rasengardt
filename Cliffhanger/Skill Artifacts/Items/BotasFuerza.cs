// Botas de la Fuerza.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Regions;
using Server.Targeting;

namespace Server.Items
{

	public class BotasFuerza : BaseShoes, SkillArtifact
	{
                public static TimeSpan delay = TimeSpan.FromMinutes( 3 );

                private static int BonusAnatomy = 5;
                private static int BonusTactics = 5;
                private static int BonusFocus = 5;
		private SkillMod smod1 = new DefaultSkillMod( SkillName.Anatomy, true, BonusAnatomy );
  		private SkillMod smod2 = new DefaultSkillMod( SkillName.Tactics, true, BonusTactics );
                private SkillMod smod3 = new DefaultSkillMod( SkillName.Focus, true, BonusFocus );

		public override int FireResistance{ get{ return 5; } }
		
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
                        		m_TipoBase = typeof(Boots);
				else
                        		m_TipoBase = value;

				ConfigurarTipoBase();
				InvalidateProperties();
			}
		}

		private void ConfigurarTipoBase()
		{
			Item boots = Activator.CreateInstance( m_TipoBase ) as Item;

			if( boots == null )
				return;

  			ItemID = boots.ItemID;

			Weight = boots.Weight;
			Layer = boots.Layer;

			boots.Delete();
		}
		
		[Constructable]
		public BotasFuerza() : this( SAUtility.TipoBaseRandom( typeof(BotasFuerza) ) )
		{
		}
		
		[Constructable]
		public BotasFuerza( Type tipobotas ) : base( 0x13B )
		{
			TipoBase = tipobotas;
			SetProps( null );
  		}
  		
		public BotasFuerza( BaseShoes BotasBase ) : base( 0x13B )
		{
			Type tipo = null;
			
			if( BotasBase != null )
				tipo = BotasBase.GetType();

			TipoBase = tipo;
			SetProps( BotasBase as BaseClothing );
		}

                public BotasFuerza( Serial serial ) : base( serial )
		{
		}
		
		private void SetProps( BaseClothing RopaBase )
		{
			if( RopaBase != null )
			{
				SAUtility.CopyAosAttributes( RopaBase, this as BaseClothing );
			}
			
			Hue = 2111;
                        Name = SAUtility.NombreArtefacto( GetType() );
                        Attributes.BonusHits = 10;
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
                        	from.SendMessage("Debes tener equipadas las Botas para usarlas.");
                        	return;
                        }

			if( ((dovPlayerMobile)from).BFLastDobleClick + delay > DateTime.Now )
			{
                        	from.SendMessage("Debes esperar 3 minutos para volver a usar las Botas.");
                        	return;
                        }
                        
                        if ( !SkillRequerida( from ) )
                        {
                        	from.SendMessage("Usar la fuerza de las botas requiere 80 de Tactics.");
                        	return;
                        }
                        
                        if( from.Stam < 20 )
			{
                        	from.SendMessage("Necesitas 20 de estamina para realizar esta accion.");
                        	return;
                        }
                        
                        if ( from.Region is Jail )
                        {
                        	from.SendMessage("No puedes usar las botas aca.");
                        	return;
                        }
                        
                        from.RevealingAction();
			GenerarTerremoto( from );
		}
                
                private void GenerarTerremoto( Mobile from )
                {

 			ArrayList targets = new ArrayList();

			Map map = from.Map;

			if ( map == null )
				return;

			foreach ( Mobile m in from.GetMobilesInRange( 7 ) )
			{
				if ( from != m && SpellHelper.ValidIndirectTarget( from, m ) && from.CanBeHarmful( m, false ) )
					targets.Add( m );
			}

                        if( targets.Count == 0 )
                        {
                        	from.SendMessage("No hay enemigos validos en tu rango.");
                        	return;
                        }
                        
                        from.LocalOverheadMessage( MessageType.Regular, 2111, true, "Tus Botas han generado un terremoto!");
			from.FixedParticles( 0x37BE, 1, 15, 9961, 2111, 0, EffectLayer.RightHand );

                        for ( int i = 0; i < targets.Count; ++i )
			{
				Mobile m = (Mobile)targets[i];

				int damage = Utility.RandomMinMax( 15, 20 );
                        
				from.DoHarmful( m );

                                m.FixedParticles( 0x3709, 10, 30, 9961, 2111, 0, EffectLayer.LeftFoot );
				m.PlaySound( 0x2F3 );
				m.SendMessage("Has sido alcanzado por el terremoto de las botas Enanas!");
				m.Damage( damage, from );
			}

			from.Freeze( TimeSpan.FromSeconds( 3 ) );
			from.Stam -= 20;

			((dovPlayerMobile)from).BFLastDobleClick = DateTime.Now;

                }
                
                public bool SkillRequerida( Mobile m )
		{
			if ( m.Skills.Tactics.Value < 80.0 )
				return false;

			return true;
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
                	from.SendMessage( "No puedes hacer eso en este item!" );
			return false;
  		}

		private void ActivarBonus( Mobile m )
                {
                 	if ( m == null )
                 		return;

                 	m.AddSkillMod( smod1 );
			m.AddSkillMod( smod2 );
			m.AddSkillMod( smod3 );
                }

                private void DesactivarBonus( Mobile m )
                {
                        smod1.Remove();
                        smod2.Remove();
                        smod3.Remove();
                }

                public override bool OnEquip( Mobile from )
                {
                        if( SAUtility.MuchosEquipados( from ) )
                        {
                        	from.SendMessage( "No puedes equiparte otro artefacto." );
                        	return false;
                        }

			ActivarBonus( from );

                	return base.OnEquip( from );
                }

                public override void OnRemoved( object parent )
                {
			if ( !(parent is Mobile) )
				return;

                        DesactivarBonus( (Mobile)parent );

                        base.OnRemoved( parent );
		}
		
              	public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060660, "Bonus Anatomy\t" + BonusAnatomy.ToString() + "\nBonus Tactics: " + BonusTactics.ToString() + "\nBonus Focus: " + BonusFocus.ToString() );
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

			ActivarBonus( Parent as Mobile );
			this.Hue = 2111;
		}
	}
}
