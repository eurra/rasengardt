using System;
using Server;
using Server.Guilds;
using Server.Mobiles;
using Server.Gumps;
using Server.Targeting;
using Server.Network;
using Server.Items;

namespace Server.items
{
	public class GuildDyeGump : Gump
	{
        	private Guild m_Guild;
        	private GuildDye m_GuildDye;
        	
        	public GuildDyeGump( Mobile from, Guild guild, GuildDye guilddye ) : base( 0, 0 )
		{
			from.CloseGump( typeof( GuildDyeGump ) );

			m_Guild = guild;
			m_GuildDye = guilddye;

			Closable = false;
			Disposable = false;
			Dragable = true;
			Resizable = false;

			AddPage(0);

			if( m_Guild != null && m_Guild.CustomHue2 != 0 )
			{
				AddBackground(122, 80, 250, 270, 9250);
				AddButton(325, 313, 4017, 4018, 2, GumpButtonType.Reply, 0);
				AddLabel(280, 313, 0x480, "SALIR");
			}
			else
			{
                                AddBackground(122, 80, 250, 190, 9250);
                                AddButton(325, 235, 4017, 4018, 2, GumpButtonType.Reply, 0);
				AddLabel(280, 235, 0x480, "SALIR");
			}

			AddLabel(211, 94, 54, "GUILD DYE");
			AddLabel(164, 121, 0x480, "Quiero pintar una prenda...");

                        if( m_Guild != null && m_Guild.CustomHue1 != 0 )
			{
				AddImage(291, 157, 111, m_Guild.CustomHue1 - 1 );
				AddLabel(145, 171, 54, "Con el color primario.");
				AddButton(196, 195, 4005, 4006, 0, GumpButtonType.Reply, 0);
			}

			if( m_Guild != null && m_Guild.CustomHue2 != 0 )
			{
				AddImage(291, 232, 111, m_Guild.CustomHue2 - 1 );
				AddLabel(145, 246, 54, "Con el color secundario.");
				AddButton(196, 270, 4005, 4006, 1, GumpButtonType.Reply, 0);
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if( m_Guild == null || m_GuildDye == null )
				return;
			
			Mobile from = state.Mobile;

			if( !m_Guild.IsMember( from ) )
				return;
				
			if ( !from.InRange( m_GuildDye.GetWorldLocation(), 1 ) )
			{
				from.SendMessage( "El tub seleccionado esta muy lejos" );
				return;
			}

			switch ( info.ButtonID )
			{
				case 0:
				{
					if( m_Guild.CustomHue1 == 0 )
						return;
						
					from.SendMessage( "Selecciona la prenda que quieres teñir con el color primario de tu guild..." );
					from.Target = new InternalTarget( m_Guild, m_GuildDye, true );

					break;
				}
                                case 1:
				{
					if( m_Guild.CustomHue2 == 0 )
						return;
						
					from.SendMessage( "Selecciona la prenda que quieres teñir con el color secundario de tu guild..." );
					from.Target = new InternalTarget( m_Guild, m_GuildDye, false );

					break;
				}
				case 2:
				{
					return;
				}
			}
		}

		private class InternalTarget : Target
		{
			private Guild m_Guild;
			private GuildDye m_GuildDye;
			private bool m_Primario;

			public InternalTarget( Guild guild, GuildDye guilddye, bool primario ) : base( 1, false, TargetFlags.None )
			{
				m_Guild = guild;
				m_GuildDye = guilddye;
				m_Primario = primario;
			}
			
			protected override void OnTarget( Mobile from, object targeted )
			{
				if( m_Guild == null || m_GuildDye == null )
					return;

				if( !m_Guild.IsMember( from ) )
					return;
					
				if( !( targeted is BaseClothing ) )
				{
					from.SendMessage( "Por ahora sólo puedes pintar prendas de vestir con este item." );
					return;
				}
				
				BaseClothing cloth = (BaseClothing)targeted;

                                if( !from.InRange( m_GuildDye.GetWorldLocation(), 1 ) || !from.InRange( cloth.GetWorldLocation(), 1 ) )
                                {
					from.SendMessage( "El tub o la ropa estan muy lejos." );
					return;
				}

				if( m_Primario )
				{
					if( m_Guild.CustomHue1 == 0 )
						return;

                                	cloth.Hue = m_Guild.CustomHue1;
                                }
                                else
                                {
                                	if( m_Guild.CustomHue2 == 0 )
						return;
						
					cloth.Hue = m_Guild.CustomHue2;
                                }

                                cloth.OwnerGuild = m_Guild;
                                from.SendMessage( "Has teñido tu prenda exitosamente, ahora esta marcada para el uso exclusivo de tu guild." );
			}
		}
	}
	
	public class GuildDye : Item
	{
		[Constructable] 
		public GuildDye() : base( 0xFAB )
		{
			Weight = 10.0;
			Name = "Guild Dye Tub";
		}
		
		public GuildDye( Serial serial ) : base( serial )
		{
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( this.GetWorldLocation(), 1 ) )
			{
				from.SendMessage( "Estas muy lejos del tub." );
				return;
			}
			
			Guild guild = from.Guild as Guild;
			
			if( guild == null )
			{
                        	from.SendMessage( "Si no perteneces a alguna guild, no puedes usar este item." );
				return;
			}
			
			if( guild.CustomHue1 == 0 )
			{
				from.SendMessage( "Tu guild no tiene configurados sus respectivos colores. Consulta con un Admin al respecto." );
				return;
			}

			from.SendGump( new GuildDyeGump( from, guild, this ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
