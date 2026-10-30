# Pruebas y capturas pendientes en Windows

Abrir el programa y seguir estos pasos. Los resultados de esta tabla son **esperados**;
no sustituyen la comprobación visual real. Guardar cada imagen con **Guardar captura PNG**.

| Archivo | Pasos | Resultado esperado |
| --- | --- | --- |
| 01_lista_enlazada.png | Lista enlazada propia → Cargar demostración | [10,15,30], tres nodos; último siguiente null; registro de inserción, búsqueda y eliminación |
| 02_busqueda.png | Después de lo anterior, escribir 30 → Buscar valor | Encontrado en posición 2; cantidad sigue en 3 |
| 03_eliminacion.png | Posición 1 → Eliminar posición | Retira 15; quedan [10,30]; enlaces ajustados |
| 04_pila.png | Pila genérica → Cargar demostración → Consultar | Retira 30; vista [20,10]; consulta 20; cantidad 2 |
| 05_cola.png | Cola genérica → Cargar demostración → Consultar | Retira 10; vista [20,30]; consulta 20; cantidad 2 |
| 06_reserva_lista.png | Lista genérica → Cargar demostración | Siete datos; Count=7; Capacity>=7 (en la ejecución de consola fue 8) |
| 07_limpiar_lista.png | En Lista genérica → Limpiar | Count=0 y la capacidad anterior se conserva |
| 08_compactar.png | Después de Limpiar → Compactar | Count=0, Capacity=0 |
| 09_espacios_sin_usar.png | Estático vs dinámico → Limpiar ambos; agregar 10,20,30 | Arreglo 3/5 con dos espacios libres; lista con tres nodos |
| 10_comparacion.png | En Estático vs dinámico → Cargar demostración | Arreglo conserva 10 a 50; lista acepta 10 a 80; tres entradas rechazadas solo por el arreglo |
| 11_posicion_invalida.png | Lista enlazada propia → Limpiar; posición 1 → Eliminar posición | Rechazo; lista permanece vacía |

También verificar letras en el campo de valor, cero, negativos y retiro/consulta en pila
y cola vacías. Redimensionar la ventana y comprobar que se vean acciones, tabla y registro.
Si DrawToBitmap no incluye correctamente un control, usar Recortes de Windows para
capturar la ventana. Registrar sistema, versión de .NET, fecha y resultado observado.

El guion del reporte se realiza íntegramente en el programa. Las capturas 1, 4, 5, 6, 7,
8, 9 y 10 cubren sus demostraciones principales.
