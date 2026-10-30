using System;
using System.Collections.Generic;

namespace Unidad3.Dinamicas;

// Lista simplemente enlazada propia: no utiliza LinkedList<T> ni List<T> para guardar nodos.
public sealed class ListaEnlazada<T>
{
    // Cada instancia conserva un dato y la referencia al siguiente nodo.
    // La clase privada impide alterar los enlaces desde el formulario.
    private sealed class Nodo
    {
        public T Valor;
        public Nodo? Siguiente;
        public Nodo(T valor) { Valor = valor; }
    }

    private Nodo? cabeza;
    private Nodo? cola; // Evita recorrer toda la lista al agregar al final.
    public int Cantidad { get; private set; }
    public bool EstaVacia => Cantidad == 0;

    public void Agregar(T valor)
    {
        Nodo nuevo = new Nodo(valor);
        if (cola is null) cabeza = nuevo;
        else cola.Siguiente = nuevo;
        cola = nuevo;
        Cantidad++;
    }

    public void Insertar(int posicion, T valor)
    {
        // Se permite Cantidad para insertar después del último elemento.
        if (posicion < 0 || posicion > Cantidad)
            throw new ArgumentOutOfRangeException(nameof(posicion), "Posición de inserción inválida.");
        if (posicion == Cantidad) { Agregar(valor); return; }
        Nodo nuevo = new Nodo(valor);
        if (posicion == 0)
        {
            nuevo.Siguiente = cabeza;
            cabeza = nuevo;
        }
        else
        {
            Nodo anterior = NodoEn(posicion - 1);
            nuevo.Siguiente = anterior.Siguiente;
            anterior.Siguiente = nuevo;
        }
        Cantidad++;
    }

    public T Eliminar(int posicion)
    {
        ValidarExistente(posicion);
        Nodo eliminado;
        if (posicion == 0)
        {
            eliminado = cabeza!;
            cabeza = eliminado.Siguiente;
            if (Cantidad == 1) cola = null;
        }
        else
        {
            Nodo anterior = NodoEn(posicion - 1);
            eliminado = anterior.Siguiente!;
            anterior.Siguiente = eliminado.Siguiente;
            if (posicion == Cantidad - 1) cola = anterior;
        }
        eliminado.Siguiente = null; // Desenlazar; el GC decide cuándo recuperar el objeto.
        Cantidad--;
        return eliminado.Valor;
    }

    public T Obtener(int posicion) => NodoEn(posicion).Valor;

    public int Buscar(T valor)
    {
        Nodo? actual = cabeza;
        int posicion = 0;
        while (actual is not null)
        {
            // Comparador genérico: admite valores repetidos y referencias null.
            if (EqualityComparer<T>.Default.Equals(actual.Valor, valor)) return posicion;
            actual = actual.Siguiente;
            posicion++;
        }
        return -1; // La búsqueda devuelve la primera coincidencia o -1.
    }

    public T[] AArreglo()
    {
        // Copia para mostrar/probar el estado; el almacenamiento principal sigue en nodos.
        T[] resultado = new T[Cantidad];
        Nodo? actual = cabeza;
        for (int i = 0; i < resultado.Length; i++)
        {
            resultado[i] = actual!.Valor;
            actual = actual.Siguiente;
        }
        return resultado;
    }

    public void Limpiar()
    {
        // Al perder las referencias de entrada, los nodos quedan disponibles para el GC.
        cabeza = null; cola = null; Cantidad = 0;
    }

    private Nodo NodoEn(int posicion)
    {
        ValidarExistente(posicion);
        Nodo actual = cabeza!;
        for (int i = 0; i < posicion; i++) actual = actual.Siguiente!;
        return actual;
    }

    private void ValidarExistente(int posicion)
    {
        if (posicion < 0 || posicion >= Cantidad)
            throw new ArgumentOutOfRangeException(nameof(posicion), "No existe un elemento en esa posición.");
    }
}
