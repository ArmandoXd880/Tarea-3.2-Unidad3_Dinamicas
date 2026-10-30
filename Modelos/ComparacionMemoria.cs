using System;

namespace Unidad3.Dinamicas;

// Aplica las mismas entradas a dos estructuras independientes para observar sus límites.
public sealed class ComparacionMemoria
{
    private readonly int[] fijo;
    private readonly ListaEnlazada<int> enlazada = new ListaEnlazada<int>();
    public int CapacidadFija => fijo.Length;
    public int CantidadFija { get; private set; }
    public int EspaciosSinUsar => CapacidadFija - CantidadFija;
    public int CantidadDinamica => enlazada.Cantidad;

    public ComparacionMemoria(int capacidad)
    {
        if (capacidad <= 0) throw new ArgumentOutOfRangeException(nameof(capacidad));
        fijo = new int[capacidad];
    }

    public bool Agregar(int valor)
    {
        enlazada.Agregar(valor); // La lista crea un nodo por elemento aceptado.
        if (CantidadFija == CapacidadFija) return false;
        fijo[CantidadFija++] = valor;
        return true; // Indica si el arreglo fijo también pudo aceptar la entrada.
    }

    public int[] ValoresFijos()
    {
        int[] copia = new int[CantidadFija];
        Array.Copy(fijo, copia, CantidadFija);
        return copia;
    }
    public int[] ValoresDinamicos() => enlazada.AArreglo();

    public void Limpiar()
    {
        Array.Clear(fijo, 0, fijo.Length); CantidadFija = 0; enlazada.Limpiar();
    }
}
