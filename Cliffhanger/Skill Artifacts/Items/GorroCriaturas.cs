// Gorro de las Criaturas.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Regions;
using Server.Targeting;

namespace Server.Items
{
	public class GorroCriaturas : BaseHat, SkillArtifact
	{
                private BaseCreature m_Controlada = null;
                public int prevHue;
                public int prevControlSlots;
                
                public BaseCreature Controlada{ get{ return m_Controlada; } set{ m_Controlada = value; } }

                private static int TControl = 1; // Tiempo controlado en minutos
                private InternalTimer timer;
                //private IBarHueTimer huetimer;
                
                private static int BonusLore = 5;
                private static int BonusTaming = 5;
                private static int BonusTailor = 5;
                private static int BonusFishing = 5;
                private SkillMod smod1 = new DefaultSkillMod( SkillName.AnimalLore, true, BonusLore );
                private SkillMod smod2 = new DefaultSkillMod( SkillName.AnimalTaming, true, BonusTaming );
                private SkillMod smod3 = new DefaultSkillMod( SkillName.Tailoring, true, BonusTailor );
                private SkillMod smod4 = new DefaultSkillMod( SkillName.Fishing, true, BonusFishing );

		public static TimeSpan delay = TimeSpan.FromMinutes( 5 );

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
                        		m_TipoBase = typeof(BearMask);
				else
                        		m_TipoBase = value;

				ConfigurarTipoBase();
				InvalidateProperties();
			}
		}

		private void ConfigurarTipoBase()
		{
			Item mask = Activator.CreateInstance( m_TipoBase ) as Item;

			if( mask == null )
				return;

  			ItemID = mask.ItemID;

			Weight = mask.Weight;
			Layer = mask.Layer;

			mask.Delete();
		}

		[Constructable]
		public GorroCriaturas() : this( SAUtility.TipoBaseRandom( typeof(GorroCriaturas) ) )
		{
		}
		
		[Constructable]
		public GorroCriaturas( Type tipogorro ) : base( 0x13B )
		{
			TipoBase = tipogorro;
			SetProps( null );
  		}
  		
		public GorroCriaturas( BaseHat GorroBase ) : base( 0x13B )
		{
			Type tipo = null;
			
			if( GorroBase != null )
				tipo = GorroBase.GetType();

			TipoBase = tipo;
			SetProps( GorroBase as BaseClothing );
		}

                public GorroCriaturas( Serial serial ) : base( serial )
		{
		}
		
 		private void SetProps( BaseClothing RopaBase )
		{
			if( RopaBase != null )
			{
				SAUtility.CopyAosAttributes( RopaBase, this as BaseClothing );
			}

		        Hue = 1357;
                        Name = SAUtility.NombreArtefacto( GetType() );
                        Attributes.BonusMana = 5;
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
                        	from.SendMessage("Debes tener equipado el Gorro para usarlo.");
                        	return;
                        }

			if( ((dovPlayerMobile)from).GCLastDobleClick + delay > DateTime.Now )
			{
                        	from.SendMessage("Debes esperar 5 Minutos antes de usar nuevamente el Gorro.");
                        	return;
                        }
                        
                        if ( !SkillRequerida(from) )
                        {
                        	from.SendMessage("Necesitas 80 de Animal Taming para usar el gorro.");
                        	return;
                        }
                        
                        if ( from.Region is Jail )
                        {
                        	from.SendMessage("No puedes usar el gorro aca.");
                        	return;
                        }

			from.Target = new InternalTarget( from, this );
                        from.SendMessage("Selecciona la criatura que deseas controlar");
		}

		private class InternalTarget : Target
		{
			private GorroCriaturas gorro;

			public InternalTarget( Mobile m, GorroCriaturas am ) : base( 12, false, TargetFlags.None )
			{
                        	gorro = am;
                        }

                        protected override void OnTarget( Mobile from, object o )
                        {
                        
                        	if ( !(o is BaseCreature) || (o is BaseVillaGuard) || !from.CanBeHarmful( (Mobile)o ) )
                        	{
                        		from.SendMessage("No puedes controlar eso!");
                        		return;
                        	}
                        	
                        	if( !from.InLOS( o as Mobile ) )
                        	{
                        		from.SendMessage("La criatura no esta a tu alcance.");
                        		return;
                        	}
                        	
                        	BaseCreature c = (BaseCreature)o;

                        	if ( c.Controled || c.Summoned )
                        	{
                                        from.SendMessage("Esa criatura ya tiene un dueño!");
                                        return;
                                }

				int slots = GorroCriaturas.ObtenerSlots( c );

				if ( slots < 0 )
				{
                                        from.SendMessage("Esa criatura es muy poderosa para el poder del Gorro.");
                                        return;
                                }

                                if ( (from.Followers + slots) > from.FollowersMax )
                                {
                                        from.SendMessage("Tienes demasiados seguidores como para controlar esta criatura.");
                                        return;
                                }

                                // Conversion

				((dovPlayerMobile)from).GCLastDobleClick = DateTime.Now;
				from.RevealingAction();

				gorro.prevControlSlots = c.ControlSlots;
				c.ControlSlots = slots;

				gorro.prevHue = c.Hue;
				c.Hue = 1357;

				c.SetControlMaster( from );
				c.ControlOrder = OrderType.Guard;

				gorro.Controlada = c;
				gorro.timer = new InternalTimer( gorro );
				gorro.timer.Start();

				//gorro.huetimer = new IBarHueTimer ( gorro as Item );
				//gorro.huetimer.Start();
				
				c.PlaySound( 521 );
                        	c.FixedParticles( 0x3789, 1, 40, 9910, 1357, 0, EffectLayer.LeftHand );
                        	from.LocalOverheadMessage( MessageType.Regular, 1357, true, "Has logrado controlar la criatura!");
                        }
                }

		private class InternalTimer : Timer
		{
			private GorroCriaturas gorro;

			public InternalTimer( GorroCriaturas am ) : base( TimeSpan.FromMinutes( GorroCriaturas.TControl ) )
			{
                                gorro = am;
                        }

                        protected override void OnTick()
                        {
                        	gorro.LiberarCriatura();
                        }
                }
                
		public void LiberarCriatura()
		{
			if ( m_Controlada != null && m_Controlada.Alive )
			{
				m_Controlada.ControlMaster.LocalOverheadMessage( MessageType.Regular, 1357, true, "Has perdido el control sobre la criatura!");
				m_Controlada.SetControlMaster( null );
				m_Controlada.Hue = prevHue;
				m_Controlada.ControlSlots = prevControlSlots;
				m_Controlada.PlaySound( 521 );
                        	m_Controlada.FixedParticles( 0x3789, 1, 40, 9910, 1357, 0, EffectLayer.LeftHand );

                        	if( m_Controlada is BaseMount )
    					((BaseMount)m_Controlada).Rider = null;
    			}

    			m_Controlada = null;

                        if( timer != null )
                        {
				timer.Stop();
				timer = null;
			}

                        /*if( huetimer != null )
                        {
				huetimer.Stop();
				huetimer = null;
                                this.Hue = 1357;
			} */
		}

		private static int ObtenerSlots( BaseCreature c )
		{
			int suma = c.Str + c.Dex + c.Int;

                        if ( suma > 800 && suma <= 1200 )
				return 4;
                        else if ( suma > 400 && suma <= 800 )
                                return 3;
                        else if ( suma <= 400 )
                        	return 2;

                        return -1;
                }

		public static bool EnGorro( Mobile master, BaseCreature creat )
		{
                	GorroCriaturas gorro = master.FindItemOnLayer( Layer.Helm ) as GorroCriaturas;

			if ( gorro == null )
				return false;
				
			if ( gorro.Controlada != creat )
				return false;

			return true;
		}
		
		public bool SkillRequerida( Mobile m )
                {
                	if ( m.Skills.AnimalTaming.Value < 80.0 )
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
                	from.SendMessage("No puedes hacer eso en este item!");
			return false;
  		} 

		private void AddMods( Mobile from )
                {
                	if( from == null )
                		return;

			from.AddSkillMod( smod1 );
                        from.AddSkillMod( smod2 );
                        from.AddSkillMod( smod3 );
                        from.AddSkillMod( smod4 );
                }

		public override bool OnEquip( Mobile from )
                {
                        if( SAUtility.MuchosEquipados( from ) )
                        {
                        	from.SendMessage( "No puedes equiparte otro artefacto." );
                        	return false;
                        }

			AddMods( from );

                	return base.OnEquip( from );
                }

                public override void OnRemoved( object parent )
                {
			if ( !(parent is Mobile) )
				return;

                        if ( smod1 != null)
				smod1.Remove();
			if ( smod2 != null)
				smod2.Remove();
			if ( smod3 != null)
				smod3.Remove();
			if ( smod4 != null)
				smod4.Remove();

                        if ( Controlada != null )
                        	LiberarCriatura();

                        base.OnRemoved( parent );
		}

 		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060660, "Bonus Animal Lore\t" + BonusLore.ToString() + "\nBonus Animal Taming: " + BonusTaming.ToString() + "\nBonus Tailoring: " + BonusTailor.ToString() + "\nBonus Fishing: " + BonusFishing.ToString() );
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

			AddMods( Parent as Mobile );
			this.Hue = 1357;
		}

	}

}
