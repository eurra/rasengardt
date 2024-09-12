using System;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Accounting;

namespace Server.Jailing
{
	public class JailTarget : Target
	{
		public JailTarget() : base( -1, false, TargetFlags.None )
		{
		}

		protected override void OnTarget( Mobile from, object o )
		{
                	if( !( o is Mobile ) )
                        {
                              	from.SendMessage( "No puedes encarcelar eso!" );
                               	from.SendGump( new JailGump( from, Pages.PageEncarcelar ) );
                                return;
                        }

                        Mobile m = o as Mobile;

			if( !m.Player )
                        {
                         	from.SendMessage( "Solo pueden encarcelarce jugadores!" );
                         	from.SendGump( new JailGump( from, Pages.PageEncarcelar ) );
                         	return;
                        }

			JailInfo info = JailSystem.GetJailing( m.Account as Account );

			if( info != null )
			{
				from.SendMessage("La cuenta señalada ya esta encarcelada.");
                                from.SendGump( new JailGump( from, Pages.PageEncarcelar ) );
				return;
			}

			info = JailSystem.Jail( m.Account as Account, from );

			if( info == null )
                        {
                        	from.SendMessage( "No se ha podido realizar el encarcelamiento, cuenta inválida." );
                        	from.SendGump( new JailGump( from, Pages.PageEncarcelar ) );
                        	return;
                        }
                        else
                        {
				m.SendMessage( "Tu cuenta ha sido encarcelada!" );
                                from.SendGump( new JailGump( from, Pages.PageDatos, info ) );
                        }
		}
		
		protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
		{
                	from.SendGump( new JailGump( from, Pages.PageEncarcelar ) );
		}
	}

	public class UnJailTarget : Target
	{
		public UnJailTarget() : base( -1, false, TargetFlags.None )
		{
		}
	
		protected override void OnTarget( Mobile from, object o )
		{
                        if( !( o is Mobile ) )
                        {
                                from.SendMessage( "No puedes desencarcelar eso!" );
                                from.SendGump( new JailGump( from, Pages.PageDesencarcelar ) );
                                return;
                        }

                        Mobile m = o as Mobile;

			if( !m.Player )
                        {
                                from.SendMessage( "Solo pueden desencarcelarce jugadores!" );
                                from.SendGump( new JailGump( from, Pages.PageDesencarcelar ) );
                                return;
                        }

			Account acc = (Account)m.Account;
					
                        if( acc == null )
                        {
                                from.SendMessage( "La cuenta del pj es inválida." );
                                from.SendGump( new JailGump( from, Pages.PageDesencarcelar ) );
                                return;
                        }

			JailInfo info = JailSystem.GetJailing( acc );

			if( info == null )
                        {
                                from.SendMessage( "El pj seleccionado no tiene su cuenta encarcelada." );
                                from.SendGump( new JailGump( from, Pages.PageEncarcelar ) );
                                return;
                        }

                        if( !JailSystem.UnJail( acc ) )
                        {
                                from.SendMessage( "No se ha podido realizar el desencarcelamiento." );
                                from.SendGump( new JailGump( from, Pages.PageDesencarcelar ) );
                                return;
                        }
                        else
                        {
                                from.SendGump( new JailGump( from, Pages.PagePrincipal ) );
                                from.SendMessage("Desencarcelamiento realizado correctamente.");
                        }
		}
		
		protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
		{
                	from.SendGump( new JailGump( from, Pages.PageDesencarcelar ) );
		}
	}
			
	public class JailInfoTarget : Target
	{
		public JailInfoTarget() : base( -1, false, TargetFlags.None )
		{
		}
	
		protected override void OnTarget( Mobile from, object o )
		{
                        if( !( o is Mobile ) )
                        {
                                from.SendMessage( "No puedes seleccionar eso!" );
                                from.SendGump( new JailGump( from, Pages.PageMostrarDatos ) );
                                return;
                        }

                        Mobile m = o as Mobile;

			if( !m.Player )
                        {
                                from.SendMessage( "Solo puedes seleccionar jugadores!" );
                                from.SendGump( new JailGump( from, Pages.PageMostrarDatos ) );
                                return;
                        }
                                        
                        Account acc = (Account)m.Account;
					
                        if( acc == null )
                        {
                                from.SendMessage( "La cuenta del pj es inválida." );
                                from.SendGump( new JailGump( from, Pages.PageMostrarDatos ) );
                                return;
                        }

			JailInfo info = JailSystem.GetJailing( acc );

			if( info == null )
                        {
                                from.SendMessage( "El pj seleccionado no tiene su cuenta encarcelada." );
                                from.SendGump( new JailGump( from, Pages.PageMostrarDatos ) );
                                return;
                        }

                        from.SendGump( new JailGump( from, Pages.PageDatos, info ) );
		}
		
                protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
		{
                	from.SendGump( new JailGump( from, Pages.PageMostrarDatos ) );
		}
	}
}
