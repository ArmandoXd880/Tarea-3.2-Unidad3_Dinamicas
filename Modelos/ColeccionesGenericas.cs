using System;
using System.Collections.Generic;

namespace Unidad3.Dinamicas;

// Adaptadores para mostrar tres colecciones predefinidas mediante una vista común.
public interface IColeccionDinamica
{
    string Nombre { get; }
    string Explicacion { get; }
    string Detalle { get; }
    int Cantidad { get; }
    void Agregar(int valor);
    int Retirar();
    int Consultar();
    void Limpiar();
    int[] Valores();
}

public sealed class PilaGenerica : IColeccionDinamica
{
    private readonly Stack<int> datos = new Stack<int>();
    public string Nombre => "Pila genérica";
    public string Explicacion => "Stack<int> · LIFO · la tabla empieza por la cima.";
    public string Detalle => "Push inserta; Pop retira; Peek consulta la cima.";
    public int Cantidad => datos.Count;
    public void Agregar(int valor) => datos.Push(valor);
    public int Retirar() { Validar(); return datos.Pop(); }
    public int Consultar() { Validar(); return datos.Peek(); }
    public void Limpiar() => datos.Clear();
    public int[] Valores() => datos.ToArray(); // Stack copia en orden desde la cima.
    private void Validar()
    {
        if (datos.Count == 0) throw new InvalidOperationException("Pila vacía.");
    }
}

public sealed class ColaGenerica : IColeccionDinamica
{
    private readonly Queue<int> datos = new Queue<int>();
    public string Nombre => "Cola genérica";
    public string Explicacion => "Queue<int> · FIFO · la tabla empieza por el frente.";
    public string Detalle => "Enqueue inserta; Dequeue retira; Peek consulta el frente.";
    public int Cantidad => datos.Count;
    public void Agregar(int valor) => datos.Enqueue(valor);
    public int Retirar() { Validar(); return datos.Dequeue(); }
    public int Consultar() { Validar(); return datos.Peek(); }
    public void Limpiar() => datos.Clear();
    public int[] Valores() => datos.ToArray(); // Queue copia en orden de salida FIFO.
    private void Validar()
    {
        if (datos.Count == 0) throw new InvalidOperationException("Cola vacía.");
    }
}

public sealed class ListaGenerica : IColeccionDinamica
{
    private readonly List<int> datos = new List<int>();
    public string Nombre => "Lista genérica";
    public string Explicacion => "List<int> · arreglo interno que aumenta su capacidad.";
    public string Detalle => $"Count = {datos.Count}   Capacity = {datos.Capacity}   Espacios sin usar = {datos.Capacity - datos.Count}";
    public int Cantidad => datos.Count;
    public int Capacidad => datos.Capacity;
    public void Agregar(int valor) => datos.Add(valor);
    // Elegimos retirar la posición cero para hacer visible el desplazamiento lógico.
    public int Retirar()
    {
        Validar(); int valor = datos[0]; datos.RemoveAt(0); return valor;
    }
    public int Consultar() { Validar(); return datos[0]; }
    public void Limpiar() => datos.Clear(); // Clear reduce Count; conserva Capacity.
    public void Compactar() => datos.TrimExcess(); // Puede reducir la reserva sobrante.
    public int[] Valores() => datos.ToArray();
    private void Validar()
    {
        if (datos.Count == 0) throw new InvalidOperationException("Lista vacía.");
    }
}
