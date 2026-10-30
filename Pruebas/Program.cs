using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unidad3.Dinamicas;

internal static class Program
{
    private static int comprobaciones;
    private static void Verificar(bool condicion, string mensaje)
    {
        comprobaciones++;
        if (!condicion) throw new Exception("Falló: " + mensaje);
    }
    private static void Esperar<T>(Action accion) where T : Exception
    {
        try { accion(); }
        catch (T) { comprobaciones++; return; }
        throw new Exception("Se esperaba " + typeof(T).Name);
    }
    private static void Iguales<T>(T[] real, T[] esperado)
    {
        Verificar(real.Length == esperado.Length, "longitud");
        for (int i = 0; i < esperado.Length; i++)
            Verificar(EqualityComparer<T>.Default.Equals(real[i], esperado[i]), "orden y valor");
    }
    private static void Main()
    {
        Console.WriteLine("TAREA 3.2 | Pruebas reales de los modelos en C#");
        Console.WriteLine("Entorno: " + RuntimeInformation.FrameworkDescription);
        Console.WriteLine("Sistema: " + RuntimeInformation.OSDescription);
        ListaPropia(); Genericos(); Comparacion(); Demostracion();
        Console.WriteLine($"TOTAL: {comprobaciones} verificaciones superadas.");
        Console.WriteLine("La interfaz Windows Forms no se ejecutó en este entorno.");
    }

    private static void ListaPropia()
    {
        var lista = new ListaEnlazada<int>();
        var modelo = new List<int>(); // Oráculo independiente de arreglo dinámico.
        var azar = new Random(3202);
        for (int paso = 0; paso < 2500; paso++)
        {
            int operacion = azar.Next(7); int valor = azar.Next(-20, 21);
            if (modelo.Count > 30) operacion = 2;
            switch (operacion)
            {
                case 0: lista.Agregar(valor); modelo.Add(valor); break;
                case 1:
                    int posicion = azar.Next(modelo.Count + 1);
                    lista.Insertar(posicion, valor); modelo.Insert(posicion, valor); break;
                case 2:
                    if (modelo.Count == 0) Esperar<ArgumentOutOfRangeException>(() => lista.Eliminar(0));
                    else
                    {
                        int quitar = azar.Next(modelo.Count); int esperado = modelo[quitar];
                        Verificar(lista.Eliminar(quitar) == esperado, "valor eliminado"); modelo.RemoveAt(quitar);
                    }
                    break;
                case 3: Verificar(lista.Buscar(valor) == modelo.IndexOf(valor), "primera coincidencia"); break;
                case 4:
                    Esperar<ArgumentOutOfRangeException>(() => lista.Insertar(-1, valor));
                    Esperar<ArgumentOutOfRangeException>(() => lista.Insertar(lista.Cantidad + 1, valor));
                    Esperar<ArgumentOutOfRangeException>(() => lista.Eliminar(lista.Cantidad));
                    Esperar<ArgumentOutOfRangeException>(() => lista.Obtener(-1)); break;
                case 5: lista.Limpiar(); modelo.Clear(); break;
                default:
                    if (modelo.Count > 0) Verificar(lista.Obtener(modelo.Count - 1) == modelo[modelo.Count - 1], "último");
                    else Esperar<ArgumentOutOfRangeException>(() => lista.Obtener(0));
                    break;
            }
            Verificar(lista.Cantidad == modelo.Count, "cantidad después de cada paso");
            Verificar(lista.EstaVacia == (modelo.Count == 0), "vacío después de cada paso");
            Iguales(lista.AArreglo(), modelo.ToArray());
        }
        // Eliminar cabeza/cola/único nodo y volver a agregar detecta enlaces finales incorrectos.
        lista.Limpiar(); lista.Agregar(1); lista.Agregar(2); lista.Agregar(3);
        Verificar(lista.Eliminar(2) == 3, "eliminar cola"); lista.Agregar(4);
        Iguales(lista.AArreglo(), new[] { 1, 2, 4 });
        Verificar(lista.Eliminar(0) == 1, "eliminar cabeza");
        Verificar(lista.Eliminar(0) == 2, "eliminar cabeza restante");
        Verificar(lista.Eliminar(0) == 4 && lista.EstaVacia, "eliminar único nodo");
        lista.Insertar(0, int.MinValue); lista.Agregar(int.MaxValue); lista.Agregar(0);
        Iguales(lista.AArreglo(), new[] { int.MinValue, int.MaxValue, 0 });
        int[] copia = lista.AArreglo(); copia[0] = 99;
        Verificar(lista.Obtener(0) == int.MinValue, "copia no expone nodos");
        var textos = new ListaEnlazada<string?>(); textos.Agregar(null); textos.Agregar("hola"); textos.Agregar("hola");
        Verificar(textos.Buscar(null) == 0 && textos.Buscar("hola") == 1, "genericidad y duplicados");
        Iguales(textos.AArreglo(), new string?[] { null, "hola", "hola" });
        Console.WriteLine("CORRECTO: lista enlazada propia; 2500 pasos reproducibles y límites.");
    }

    private static void Genericos()
    {
        IColeccionDinamica[] tipos = { new PilaGenerica(), new ColaGenerica(), new ListaGenerica() };
        foreach (IColeccionDinamica coleccion in tipos)
        {
            Esperar<InvalidOperationException>(() => coleccion.Retirar());
            Esperar<InvalidOperationException>(() => coleccion.Consultar());
            for (int i = 0; i < 40; i++) coleccion.Agregar(i - 20); // Superar cinco datos.
            Verificar(coleccion.Cantidad == 40, "crecimiento genérico");
            int[] esperado = new int[40];
            for (int i = 0; i < 40; i++) esperado[i] = coleccion is PilaGenerica ? 19 - i : i - 20;
            Iguales(coleccion.Valores(), esperado);
            for (int i = 0; i < esperado.Length; i++)
            {
                Verificar(coleccion.Consultar() == esperado[i], "consulta");
                Verificar(coleccion.Cantidad == 40 - i, "consulta no extrae");
                Verificar(coleccion.Retirar() == esperado[i], "salida");
            }
            Verificar(coleccion.Cantidad == 0, "vaciar colección");
            coleccion.Agregar(0); coleccion.Limpiar();
            Verificar(coleccion.Cantidad == 0, "limpieza");
        }
        var lista = new ListaGenerica(); for (int i = 0; i < 7; i++) lista.Agregar(i);
        int capacidad = lista.Capacidad;
        Verificar(capacidad >= lista.Cantidad, "Capacity mayor o igual a Count");
        lista.Limpiar(); Verificar(lista.Cantidad == 0 && lista.Capacidad == capacidad, "Clear conserva reserva");
        lista.Compactar(); Verificar(lista.Capacidad == 0, "compactar vacía");
        Console.WriteLine("CORRECTO: Stack<int>, Queue<int> y List<int>; 40 datos y orden de salida.");
    }

    private static void Comparacion()
    {
        Esperar<ArgumentOutOfRangeException>(() => new ComparacionMemoria(0));
        var modelo = new ComparacionMemoria(5);
        for (int i = 1; i <= 8; i++)
        {
            Verificar(modelo.Agregar(i * 10) == (i <= 5), "aceptación por capacidad");
            Verificar(modelo.CantidadFija == Math.Min(5, i), "cuenta fija");
            Verificar(modelo.CantidadDinamica == i, "cuenta dinámica");
        }
        Iguales(modelo.ValoresFijos(), new[] { 10, 20, 30, 40, 50 });
        Iguales(modelo.ValoresDinamicos(), new[] { 10, 20, 30, 40, 50, 60, 70, 80 });
        modelo.Limpiar(); Verificar(modelo.EspaciosSinUsar == 5 && modelo.CantidadDinamica == 0, "limpieza comparada");
        Console.WriteLine("CORRECTO: comparación; arreglo acepta 5 y lista enlazada acepta 8.");
    }

    private static void Demostracion()
    {
        var lista = new ListaEnlazada<int>(); lista.Agregar(10); lista.Agregar(20); lista.Agregar(30);
        lista.Insertar(1, 15); int encontrado = lista.Buscar(20); int retirado = lista.Eliminar(2);
        Iguales(lista.AArreglo(), new[] { 10, 15, 30 });
        Console.WriteLine($"DEMO enlazada: buscar 20 -> posición {encontrado}; eliminar -> {retirado}; [10,15,30].");
        var pila = new PilaGenerica(); var cola = new ColaGenerica();
        foreach (int valor in new[] { 10, 20, 30 }) { pila.Agregar(valor); cola.Agregar(valor); }
        Console.WriteLine($"DEMO genéricas: pila retira {pila.Retirar()}; cola retira {cola.Retirar()}.");
        var reserva = new ListaGenerica(); for (int i = 1; i <= 7; i++) reserva.Agregar(i * 10);
        Console.WriteLine($"DEMO List<int>: Count={reserva.Cantidad}; Capacity={reserva.Capacidad}; sin usar={reserva.Capacidad - reserva.Cantidad}.");
        reserva.Limpiar(); Console.WriteLine($"Después de Limpiar: Count={reserva.Cantidad}; Capacity={reserva.Capacidad}.");
        reserva.Compactar(); Console.WriteLine($"Después de Compactar: Count={reserva.Cantidad}; Capacity={reserva.Capacidad}.");
    }
}
