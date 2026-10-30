# Tarea 3.2 · Estructuras dinámicas y comparación

**Alumno:** Jesús Armando Zendejo Altamirano  
**Materia:** Estructuras de Datos · Unidad 3  
**Docente:** Ruben Riojas

Aplicación en **C# y Windows Forms**, con código comentado en español. Incluye una
lista simplemente enlazada genérica implementada desde cero y ejemplos de uso de
`Stack<int>`, `Queue<int>` y `List<int>`. Una quinta pestaña compara un arreglo fijo
de cinco posiciones con una lista enlazada dinámica.

## Ejecutar en Windows

Instalar el SDK de .NET 8 o Visual Studio con **Desarrollo de escritorio de .NET**.
Abrir `Unidad3_Dinamicas.sln`, seleccionar **Aplicacion** como proyecto de inicio
y pulsar F5. El formulario se construye en C# y no requiere archivo de diseñador.

Desde la carpeta de la solución:

```powershell
dotnet run --project Aplicacion/Aplicacion.csproj -c Release
```

## Pestañas

| Pestaña | Qué permite observar |
| --- | --- |
| Lista enlazada propia | Agregar al final, insertar por posición, eliminar, buscar, cabeza, cola y enlace siguiente |
| Pila genérica | Insertar, retirar y consultar; último en entrar, primero en salir |
| Cola genérica | Insertar, retirar y consultar; primero en entrar, primero en salir |
| Lista genérica | Agregar, retirar el primer elemento, consultar, Count y Capacity |
| Estático vs dinámico | Aplicar el mismo dato a un arreglo fijo y una lista enlazada, y comparar aceptación y reserva |

Las posiciones empiezan en cero. La tabla de nodos representa el orden lógico y los
enlaces; **no muestra direcciones reales de memoria**. El último enlace es `null`.
La posición cero de la vista de la pila es su cima. En la cola, es el frente.
En la lista genérica se eligió retirar el elemento de posición cero para la demostración.

Cada pestaña tiene **Cargar demostración**, **Guardar captura PNG** y **Guardar registro TXT**.
La demostración reinicia exclusivamente los datos de esa pestaña y su registro.

## Diferencias de almacenamiento

- `ListaEnlazada<T>` guarda los datos en objetos Nodo, cada uno con un valor y un enlace
  al siguiente. Conserva referencias a cabeza y cola, y un contador.
- `List<T>`, `Stack<T>` y `Queue<T>` son colecciones dinámicas predefinidas. En .NET utilizan
  arreglos internos que pueden reemplazarse por otros de mayor capacidad; no son listas enlazadas.
- En `List<T>`, Count indica datos presentes y Capacity la reserva interna. Limpiar usa
  Clear y conserva esa reserva. Compactar usa TrimExcess; con la lista vacía reduce Capacity a cero.
- La comparación muestra posiciones y nodos, **no mediciones de bytes**. Los nodos también
  necesitan referencias, cabeceras de objetos y alineación. Una lista dinámica puede agotar memoria.
- Eliminar un nodo lo desenlaza. El recolector de basura decide cuándo recuperar su memoria.
- Las copias a arreglos sirven para mostrar o comprobar el estado. No sustituyen el almacenamiento
  enlazado de la implementación propia.

## Modelos y métodos

`ListaEnlazada<T>`: Agregar, Insertar, Eliminar, Obtener, Buscar, AArreglo y Limpiar.
Buscar devuelve la primera posición coincidente, o -1. Eliminar devuelve el valor retirado.
Insertar admite posiciones de 0 a Cantidad; eliminar y obtener requieren 0 a Cantidad-1.
Los nodos son privados y no pueden modificarse desde el formulario.

`PilaGenerica`, `ColaGenerica` y `ListaGenerica` adaptan las colecciones predefinidas a una
vista común. `ComparacionMemoria` contiene las dos estructuras independientes de la comparación.

## Pruebas

```powershell
dotnet run --project Pruebas/Pruebas.csproj -c Release
```

Se ejecutaron **13214 verificaciones**, incluyendo 2500 pasos reproducibles de la lista
propia contra un modelo de referencia basado en `List<int>`, cambios de cabeza y cola,
lista vacía, posiciones inválidas, duplicados, null con otro tipo genérico, copias
independientes y extremos de int. Las tres colecciones predefinidas se probaron con
40 elementos para comprobar crecimiento y orden de salida. La comparación se comprobó
con ocho entradas sobre un arreglo de capacidad cinco.

`Evidencias/pruebas_reales.txt` conserva la salida real con .NET 8.0.33 en Ubuntu 24.04.3 LTS.
Los modelos, las pruebas y Windows Forms se compilaron con Roslyn del SDK 8.0.427,
tratando las advertencias como errores. La interfaz se compiló contra las referencias
Windows Desktop 8.0.33. **No se ejecutó la ventana en este entorno Linux**; su revisión
visual y las capturas quedan pendientes de realizar en Windows.

## Generar ejecutable en Windows

```powershell
dotnet publish Aplicacion/Aplicacion.csproj -c Release -r win-x64 --self-contained true -o Publicacion
```

Abrir `Publicacion/Aplicacion.exe` y conservar la carpeta completa. Este paquete entrega
el código fuente; no incluye una publicación ejecutable para Windows.

## Entrega

Entregar el **PDF comparativo**, el **código fuente** y el **enlace público del video de YouTube**.
El video debe mostrar la pantalla y la cámara en la esquina inferior derecha y publicarse
en el mismo canal de las otras tareas. El reporte incluye un guion completo para leer
mientras se utiliza el programa; no requiere mostrar código.

Si se sube el proyecto a GitHub, conservar README, solución, proyectos y evidencias.
Excluir bin, obj y archivos locales de Visual Studio. Las instrucciones de la tarea 3.2
no exigen un enlace de repositorio, pero sí que se acompañe el código fuente.

## Referencias

- Microsoft. List<T>.Capacity. https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.capacity?view=net-8.0
- Microsoft. Stack<T>. https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1
- Microsoft. Queue<T>. https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.queue-1?view=net-8.0
- Microsoft. Fundamentals of garbage collection. https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals
