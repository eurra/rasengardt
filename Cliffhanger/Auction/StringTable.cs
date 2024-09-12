// Translated to Latin American - Spanish by Joe
// For corrections, please, contact me at german@bazero.biz

// Traducido al Espa�ol - Latino Americano by Joe
// Para correcciones, por favor, contactame en german@bazero.biz

// For use with Arya's Auction System 1.7 for RunUO 1.0 RC0
// Translation Revision 2


using System;
using System.Collections;

namespace Arya.Auction
{
	/// <summary>
	/// Provides access to localized text used by the system
	/// </summary>
	public class StringTable
	{
		private Hashtable m_Table;

		public StringTable()
		{
			m_Table = new Hashtable();
			m_Table.Add( 0, "Sistema de Entrega de la Subasta" );
			m_Table.Add( 1, "Entregando..." );
			m_Table.Add( 2, "Colocar el oro en tu banco" );
			m_Table.Add( 3, "Colocar el �tem en tu banco" );
			m_Table.Add( 4, "�tem" );
			m_Table.Add( 5, "Oro" );
			m_Table.Add( 6, "Ver la pantalla de subastas" );
			m_Table.Add( 7, "Cerrar" );
			m_Table.Add( 8, "Casa de Subastas" );
			m_Table.Add( 9, "Subastar Un �tem" );
			m_Table.Add( 10, "Ver Todas Las Subastas" );
			m_Table.Add( 11, "Ver Tus Subastas" );
			m_Table.Add( 12, "Ver Tus Ofertas" );
			m_Table.Add( 13, "Ver Pendientes" );
			m_Table.Add( 14, "Salir" );
			m_Table.Add( 15, "El sistema de subastas fue detenido.  Por favor, intente luego." );
			m_Table.Add( 16, "Buscar" );
			m_Table.Add( 17, "Ordenar" );
			m_Table.Add( 18, "Pagina {0}/{1}" ); // Pagina 1/3 - used when displaying more than one page
			m_Table.Add( 19, "Mostrando {0} �tems" ); // {0} is the number of �tems displayed in an auction listing
			m_Table.Add( 20, "No hay �tems para mostrar" );
			m_Table.Add( 21, "Pagina Previa" );
			m_Table.Add( 22, "Pr�xima Pagina" );
			m_Table.Add( 23, "Un error no esperado ocurri�.  Por favor, intenta de nuevo." );
			m_Table.Add( 24, "El �tem seleccionado expiro. Por favor, refresca el listado de subastas." );
			m_Table.Add( 25, "Sistema de Mensajes de la Subasta" );
			m_Table.Add( 26, "Subasta:" );
			m_Table.Add( 27, "Ver detalles" );
			m_Table.Add( 28, "No disponible" );
			m_Table.Add( 29, "Detalles del Mensaje:" );
			m_Table.Add( 30, "Tiempo disponible para que todas las partes tomen una decisi�n: {0} d�as y {1} horas." ); m_Table.Add( 31, "La subasta no existe m�s, por lo tanto este mensaje ya no es v�lido." );
			m_Table.Add( 32, "B�squedas en la Casa de Subastas" );
			m_Table.Add( 33, "Ingresa los t�rminos a buscar (en blanco = todos los �tems)" );
			m_Table.Add( 34, "Limitar b�squeda a estas opciones:" );
			m_Table.Add( 35, "Mapas" );
			m_Table.Add( 36, "Artefactos" );
			m_Table.Add( 37, "Power y Stat Scrolls" );
			m_Table.Add( 38, "Materias Primas" );
			m_Table.Add( 39, "Joyas" );
			m_Table.Add( 40, "Armas" );
			m_Table.Add( 41, "Armaduras" );
			m_Table.Add( 42, "Escudos" );
			m_Table.Add( 43, "Regentes" );
			m_Table.Add( 44, "Pociones" );
			m_Table.Add( 45, "BOD (Large)" );
			m_Table.Add( 46, "BOD (Small)" );
			m_Table.Add( 47, "Cancelar" );
			m_Table.Add( 48, "Buscar s�lo dentro de los resultados" );
			m_Table.Add( 49, "Sistema de filtrado de la Casa de Subastas" );
			m_Table.Add( 50, "Nombre" );
			m_Table.Add( 51, "Ascendente" );
			m_Table.Add( 52, "Descendiente" );
			m_Table.Add( 53, "Fecha" );
			m_Table.Add( 54, "M�s Viejo Primero" );
			m_Table.Add( 55, "M�s Nuevo Primero" );
			m_Table.Add( 56, "Tiempo Restante" );
			m_Table.Add( 57, "M�s Corto Primero" );
			m_Table.Add( 58, "M�s Largo Primero" );
			m_Table.Add( 59, "Cantidad de Ofertas" );
			m_Table.Add( 60, "Menor Cantidad" );
			m_Table.Add( 61, "Mayor Cantidad" );
			m_Table.Add( 62, "Oferta M�xima a Realizar" );
			m_Table.Add( 63, "M�s Baja Primero" );
			m_Table.Add( 64, "M�s Alta Primero" );
			m_Table.Add( 65, "M�ximo Permitido a Ofertar" );
			m_Table.Add( 66, "Cancelar Filtrado" );
			m_Table.Add( 67, "{0} �tem(s)" );
			m_Table.Add( 68, "Precio Inicial" );
			m_Table.Add( 69, "Reserva" );
			m_Table.Add( 70, "Oferta M�s Alta" );
			m_Table.Add( 71, "Sin Ofertas" );
			m_Table.Add( 72, "Web Link" );
			m_Table.Add( 73, "{0} Dias {1} Horas" ); // 5 Days 2 Hours
			m_Table.Add( 74, "{0} Horas" ); // 18 Hours
			m_Table.Add( 75, "{0} Minutos" ); // 50 Minutes
			m_Table.Add( 76, "{0} Segundos" ); // 10 Seconds
			m_Table.Add( 77, "Pendiente" );
			m_Table.Add( 78, "No Disponible" );
			m_Table.Add( 79, "Ofertar por este �tem:" );
			m_Table.Add( 80, "Ver Ofertas" );
			m_Table.Add( 81, "Descripci�n del Vendedor" );
			m_Table.Add( 82, "Hue >>" );
			m_Table.Add( 83, "[los clientes 3D no muestran color]" );
			m_Table.Add( 84, "Esta subasta ha cerrado y no se aceptan m�s ofertas." );
			m_Table.Add( 85, "Oferta inv�lida.  Oferta no aceptada." );
			m_Table.Add( 86, "Historial de Ofertas" );
			m_Table.Add( 87, "�Qui�n?" );
			m_Table.Add( 88, "Cantidad Ofrecida" );
			m_Table.Add( 89, "Regresar a la Subasta" );
			m_Table.Add( 90, "Divisi�n Criaturas" );
			m_Table.Add( 91, "Guardar en el Establo" );
			m_Table.Add( 92, "Utiliza este ticket para guardar tu mascota en el establo." );
			m_Table.Add( 93, "Las mascotas en el establo deben ser reclamadas" ); // This and the following form one sentence
			m_Table.Add( 94, "dentro de la primera semana de haberlas guardado." );
			m_Table.Add( 95, "No pagar�s por este servicio." );
			m_Table.Add( 96, "TERMINACI�N DEL SISTEMA DE SUBASTAS" );
			m_Table.Add( 97, "<basefont color=#FFFFFF>Est�s a apunto de terminar el sistema de subastas corriendo en este mundo. Esto traer� como consecuencia que todas las subastas en curso se finalicen. Todos los �tems ser�n retornados a sus due�os originales y los ofertantes m�s altos recibir�n el dinero de regreso.<br><br>�Est�s seguro de querer hacer esto?" );
			m_Table.Add( 98, "S�, quiero terminar con el sistema" );
			m_Table.Add( 99, "NO, dejar que el sistema siga corriendo" );
			m_Table.Add( 100, "Par�emtros de Nueva Subasta" );
			m_Table.Add( 101, "Duraci�n" );
			m_Table.Add( 102, "D�as" );
			m_Table.Add( 103, "Descripci�n (Opcional)" );
			m_Table.Add( 104, "Web Link (Opcional)" );
			m_Table.Add( 106, "ACEPTO las norm�s de las subastas" ); // This and the following form one sentence
			m_Table.Add( 107, "y deseo comenzar la subasta." );
			m_Table.Add( 108, "Cancelar y salir" );
			m_Table.Add( 109, "La oferta inicial debe ser de al menos 1 moneda de oro." );
			m_Table.Add( 110, "La oferta de reserva debe ser igual o superior a la oferta inicial." );
			m_Table.Add( 111, "Una subasta debe durar al menos {0} d�as." );
			m_Table.Add( 112, "Una subasta no puede durar m�s de {0} d�as." );
			m_Table.Add( 113, "Por favor, especifica el nombre de esta subasta" );
			m_Table.Add( 114, "La oferta de reserva especifica es muy alta. Bajala o aumenta la oferta inicial." );
			m_Table.Add( 115, "El sistema fue cerrado" );
			m_Table.Add( 116, "El �tem llevado por este cheque no existe m�s por motivos ajenos al sistema de subastas" );
			m_Table.Add( 117, "El contenido del cheque fue enviado a tu cuenta de banco." );
			m_Table.Add( 118, "No se pudo agregar el �tem a tu banco.  Por favor, asegurate de tener suficiente espacio." );
			m_Table.Add( 119, "Tus poderes de Dios te permiten acceder a este cheque." );
			m_Table.Add( 120, "Este cheque s�lo puede ser usado por su due�o" );
			m_Table.Add( 121, "No se supone que remuevas este �tem en forma manual.  NUNCA." );
			m_Table.Add( 122, "Cheque de Oro del Sistema de Subastas" );
			m_Table.Add( 123, "Tu oferta ha sido superada en la subasta {0}. Tu oferta fue de {1}." );
			m_Table.Add( 124, "Sistema de subastas detenido. Devolviendo tu oferta de  {1} oro por {0}" );
			m_Table.Add( 125, "La subasta para {0} fue cancelada por ti o por el due�o. Devolvi�ndote tu oferta." );
			m_Table.Add( 126, "Tu oferta de {0} por {1} no alcanzo el precio de reserva y el vendedor cancelo la operaci�n." );
			m_Table.Add( 127, "La subasta estaba en estado pendiente porque no se alcanzo el precio de reserva o porque uno o m�s �tems fueron eliminados.  No se tomo decisi�n ni por parte del vendedor o del comprador, por tanto la subasta no fue exitosa.." );
			m_Table.Add( 128, "La subasta fue cancelada porque el �tem subastado fue removido del mundo.." );
			m_Table.Add( 129, "Has vendido {0} a trav�s del sistema de subastas. La oferta m�s alta fue de {1}." );
			m_Table.Add( 130, "{0} no es una raz�n valida para un cheque del sistema de subastas" );
			m_Table.Add( 131, "Cheque de Criaturas del Sistema de Subastas" );
			m_Table.Add( 132, "Cheque de un �tem del Sistema de Subastas" );
			m_Table.Add( 133, "Tu subasta por {0} termino SIN ofertas." );
			m_Table.Add( 134, "Tu subasta por {0} fue cancelada" );
			m_Table.Add( 135, "El sistema de subastas fue detenido y el �tem subastado te fue devuelto. ({0})" );
			m_Table.Add( 136, "La subasta fue cancelada porque los �tems subastados fueron removidos del mundo." );
			m_Table.Add( 137, "Has comprado exitosamente {0} a trav�s del sistema de subastas. Tu oferta fue de {1}." );
			m_Table.Add( 138, "{0} no es una raz�n valida para un cheque de �tem" );
			m_Table.Add( 139, "No puedes subastar criaturas que no te pertenezcan." );
			m_Table.Add( 140, "No puedes subastar criaturas muertas" );
			m_Table.Add( 141, "No puedes subastar criaturas summoneadas" );
			m_Table.Add( 142, "No puedes subastar familiars" );
			m_Table.Add( 143, "Por favor, vac�a el backpack de la criatura primero" );
			m_Table.Add( 144, "La criatura representada por este cheque no existe m�s" );
			m_Table.Add( 145, "Lo siento, estamos cerrado en este momento. Por favor, intento luego nuevamente." );
			m_Table.Add( 146, "El �tem no existe m�s" );
			m_Table.Add( 147, "Slots de Control : {0}<br>" ); // For a pet
			m_Table.Add( 148, "Bondable : {0}<br>" );
			m_Table.Add( 149, "Fuerza : {0}<br>" );
			m_Table.Add( 150, "Agilidad : {0}<br>" );
			m_Table.Add( 151, "Inteligencia : {0}<br>" );
			m_Table.Add( 152, "Cantidad: {0}<br>" );
			m_Table.Add( 153, "Usos Disponibles : {0}<br>" );
			m_Table.Add( 154, "Spell : {0}<br>" );
			m_Table.Add( 155, "Cargas : {0}<br>" );
			m_Table.Add( 156, "Crafted by {0}<br>" );
			m_Table.Add( 157, "Material : {0}<br>" );
			m_Table.Add( 158, "Calidad : {0}<br>" );
			m_Table.Add( 159, "Hit Points : {0}/{1}<br>" );
			m_Table.Add( 160, "Durabilidad : {0}<br>" );
			m_Table.Add( 161, "Protecci�n: {0}<br>" );
			m_Table.Add( 162, "Cargas de Veneno : {0} [{1}]<br>" );
			m_Table.Add( 163, "Rango : {0}<br>" );
			m_Table.Add( 164, "Da�o : {0}<br>" );
			m_Table.Add( 165, "Precisi�n<br>" );
			m_Table.Add( 166, "{0} Precisi�n<br>" );
			m_Table.Add( 167, "Slayer : {0}<br>" );
			m_Table.Add( 168, "Mapa : {0}<br>" );
			m_Table.Add( 169, "Conteo de Spells : {0}" );
			m_Table.Add( 170, "Localizaci�n Inv�lida" );
			m_Table.Add( 171, "Inv�lida" );
			m_Table.Add( 172, "El �tem seleccionado fue removido y ser� guardado en estricta seguridad" );
			m_Table.Add( 173, "Cancelas la subasta y el �tem te es devuelto" );
			m_Table.Add( 174, "Cancelas la subasta y la criatura te es devuelta" );
			m_Table.Add( 175, "No tiene suficientes slots de control libres para ofertar por esa criatura" );
			m_Table.Add( 176, "Tu oferta no es lo suficientemente grande" );
			m_Table.Add( 177, "Tu oferta no alcanza el m�nimo pedido" );
			m_Table.Add( 178, "Tu establo esta lleno.  Por favor, libera espacio antes de reclamar por esta criatura." );
			m_Table.Add( 179, "Tu oferta fue superada. Un cheque por {0} monedas de oro coins fue depositado en tu banco o en tu backpack. Mira los detalles de la subasta si deseas realizar otra oferta." );
			m_Table.Add( 180, "Tu subasta ha finalizado, pero la oferta m�xima no alcanz� el precio de reserva especificado por ti.  Puedes tomar la decisi�n de si desear vender el �tem o no.<br><br>La oferta m�s alta es {0}. El precio de reserva era {1}." );
			m_Table.Add( 181, "<br><br>Algunos de los �tems subastados, fueron eliminados en el transcurso de esta subasta.  El comprador no aceptara la nueva subasta antes de que sea completada." );
			m_Table.Add( 182, "S�, deseo vender mi �tem incluso si el precio de reserva no fue alcanzado" );
			m_Table.Add( 183, "NO, no deseo vender el �tem y deseo que me sea devuelto" );
			m_Table.Add( 184, "Tu oferta no alcanzo el precio de reserva pedido por el vendedor.  El vendedor del �tem deber� ahora determinar si vende o no el �tem.<br><br>Tu oferta fue de {1}. El precio de reserva es de {2}." );
			m_Table.Add( 185, "Cerrar este mensaje" );
			m_Table.Add( 186, "Has participado y ganado la subasta. De todos modos, por razones externas uno o m�s �tems subastados no se encuentan disponibles. Por favor, rev� la subasta utilizando el bot�n de detalles y decide si aun deseas comprar los �tems o no.<br><br>Tu oferta fue de {0}" );
			m_Table.Add( 187, "<br><br>Tu oferta no alcanzo el precio de reserva pedido por el vendedor.  El vendedor ahora deber� determinar si vende el �tem o no" );
			m_Table.Add( 188, "S�, deseo comprarlo de todos modos" );
			m_Table.Add( 189, "NO, no quiero comprar el �tem y deseo que el dinero me sea devuelto" );
			m_Table.Add( 190, "Alguno de los �tems subastados no existen m�s por razones externas.  El comprador ahora deber� decidir si completa la compra o no." );
			m_Table.Add( 191, "Por favor, selecciona el �tem a subastar..." );
			m_Table.Add( 192, "No puedes tener m�s de {0} subastas activas en tu cuenta" );
			m_Table.Add( 193, "S�lo puedes subastar �tems" );
			m_Table.Add( 194, "No puedes subastar eso" );
			m_Table.Add( 195, "Uno de los �tems a subastar no fue identificado" );
			m_Table.Add( 196, "Uno de los �tems dentro del contenedor no son aceptados por la casa de subastas" );
			m_Table.Add( 197, "No puedes vender �tems con otros �tems alojados dentro de contenedores" );
			m_Table.Add( 198, "S�lo puedes subastar �tems dentro de tu backpack o de tu cuenta bancaria" );
			m_Table.Add( 199, "No tienes suficiente dinero en el banco para colocar la oferta" );
			m_Table.Add( 200, "El sistema de subastas se encuentra detenido" );
			m_Table.Add( 201, "Eliminar" );
			m_Table.Add( 202, "Tienes una oferta en una subasta que fue cerrada por el staff.  El dinero ofertado te fue devuelto." );
			m_Table.Add( 203, "Tu subasta fue cerrada por personal del staff y el �tem te fue devuelto." );
			m_Table.Add( 204, "Tu oferta debe ser de al menos {0} m�s alta que la oferta actual" );
			m_Table.Add( 205, "No puedes subastar �tems que sean mobibles" );
			m_Table.Add( 206, "Propiedades" );
			m_Table.Add( 207, "La subasta seleccionada no se encuentra activa.  Por favor, refresca el listado de subastas." );
			m_Table.Add( 208, "�Usar Comprar Ahora?:" );
			m_Table.Add( 209, "Si decides utilizar la opci�n de Comprar Ahora, por favor, coloca un precio superior al de Reserva" );
			m_Table.Add( 210, "Comprar este �tem ahora por {0} oro" );
			m_Table.Add( 105, @"<basefont color=#FF0000>Acuerdo de las Subastas<br>
<basefont color=#FFFFFF>Completando y enviando este formulario aseguras estar de acuerdo de formar parte del sistema de subastas.  El �tem que este subastando ser� removido de tu inventario y s�lo te ser� devuelto si cancelas esta subasta, si la subasta no se completa y el �tem no es vendido, o si el staff fuerza una finalizaci�n temprana de la subasta.<br>
<basefont color=#FF0000>Oferta Inicial: <basefont color=#FFFFFF> Esta es la oferta m�nima aceptada para este �tem.  Coloca un precio razonable, y posiblemente menor al que esperas recibir por este �tem al final.<br>
<basefont color=#FF0000>Reserva:<basefont color=#FFFFFF> Este valor no ser� conocido por los ofertantes, y debes considerarlo un precio seguro para tu �tem.  Si la oferta final alcanza este valor, la venta ser� autom�ticamente finalizada por el sistema.  Si, por otro lado, la oferta m�s alta esta entre la oferta inicial y el precio de reserva, se te dar� la opci�n de poder vender este �tem o no.  Tienes 7 d�as, luego de la finalizaci�n de la subasta, para tomar esta decisi�n. Si no lo haces, el sistema de subastas asumir� que seleccionaste NO vender el �tem y te ser� devuelto, as� como el dinero al ofertante m�s alto.  Los ofertantes no podr�n nunca conocer el precio de reserva del �tem.  s�lo recibir�n una notificaci�n cuando lo hayan alcanzado o no.<br>
<basefont color=#FF0000>Duraci�n:<basefont color=#FFFFFF> Este valor especifica cuantos d�as durara la subasta desde el d�a de creaci�n.  Al final de este periodo, el sistema proceder� a finalizar la subasta, devolver el �tem y la oferta m�s alta, o esperar a tu decisi�n en caso que el precio de reserva no se haya alcanzado.<br>
<basefont color=#FF0000>Comprar Ahora:<basefont color=#FFFFFF> Estas opciones te permiten especificar un precio seguro al cual est�s dispuesto a vender tu �tem antes de la finalizaci�n de la subasta.  Si el comprador est� dispuesto a pagar este precio, podr� comprar el �tem inmediatamente sin realizar futuras ofertas.<br>
<basefont color=#FF0000>Nombre:<basefont color=#FFFFFF> Este debe ser un nombre corto definiendo el �tem que estas vendiendo.  El sistema sugerir� un nombre basado en el �tem que estas vendiendo, pero tal vez desees ponerle un nombre propio dependiendo de las circunstancias.<br>
<basefont color=#FF0000>Descripci�n:<basefont color=#FFFFFF> Puedes escribir pr�cticamente cualquier cosa que desees sobre este �tem.  Ten presentes que las propiedades de algunos �tems, que te son mostradas al colocar el puntero del mouse sobre el mismo, ser�n autom�ticamente presentadas a los ofertantes, as� que no hay necesidad de colocar esos datos.  Lo mismo sucede con las monturas que este subastando: los stats y skills del animal son autom�ticamente colocados por el sistema.<br>
<basefont color=#FF0000>Web Link:<basefont color=#FFFFFF> Puedes agregar un link externo (en la web), para esta subasta, en caso que poseas una pagina para esto y desees brindar m�s informaci�n sobre el �tem subastado<br>Una vez que aceptes enviar esta subasta ten en cuenta que NO podr�s acceder nuevamente al �tem subastado hasta que la subasta no finalice.  Por favor, estate completamente seguro de lo que esto significa antes de comenzar la subasta." );
			m_Table.Add( 211, "No tienes suficiente dinero en tu banco para comprar este �tem." );
			m_Table.Add( 212, "No tienes suficiente espacio en tu banco para realizar este deposito. Por favor, libera algo de espacio e int�ntalo de nuevo." );
			m_Table.Add( 213, "Control de Subasta" );
			m_Table.Add( 214, "Propiedades" );
			m_Table.Add( 215, "Cuenta : {0}" );
			m_Table.Add( 216, "Due�o de la Cuenta : {0}" );
			m_Table.Add( 217, "Conectado" );
			m_Table.Add( 218, "Desconectado" );
			m_Table.Add( 219, "�tem Subastado" );
			m_Table.Add( 220, "Colocar en tu banco" );
			m_Table.Add( 221, "Regresar el �tem al sistema" );
			m_Table.Add( 222, "Subasta" );
			m_Table.Add( 223, "Finalizar la subasta ahora" );
			m_Table.Add( 224, "Cerrar y retornar el �tem al due�o" );
			m_Table.Add( 225, "Cerrar y colocar el �tem en tu backpack" );
			m_Table.Add( 226, "Cerrar y eliminar el �tem" );
			m_Table.Add( 227, "Panel de Subastas del Staff" );
		}

		/// <summary>
		/// Gets the localized string for the Auction System
		/// </summary>
		public string this[int index]
		{
			get
			{
				string s = m_Table[ index ] as string;

				if ( s == null )
					return "Localizaci�n Faltante.  Informa a un GM.";
				else
					return s;
			}
		}
	}
}
