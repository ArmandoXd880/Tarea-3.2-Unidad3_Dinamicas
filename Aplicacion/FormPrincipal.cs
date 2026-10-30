using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Unidad3.Dinamicas;

namespace Unidad3.Dinamicas.Interfaz;

public sealed class FormPrincipal : Form
{
    public FormPrincipal()
    {
        Text = "Tarea 3.2 | Estructuras dinámicas y comparación";
        ClientSize = new Size(1120, 820);
        MinimumSize = new Size(1080, 800);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10F);
        AutoScaleMode = AutoScaleMode.Dpi;
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        layout.Controls.Add(new Label
        {
            Text = "Estructuras dinámicas\nLista enlazada y colecciones genéricas de C#",
            Font = new Font("Segoe UI", 17F, FontStyle.Bold),
            Dock = DockStyle.Fill, Padding = new Padding(14, 7, 0, 0)
        }, 0, 0);
        var tabs = new TabControl { Dock = DockStyle.Fill };
        Agregar(tabs, "Lista enlazada propia", new PanelEnlazada());
        Agregar(tabs, "Pila genérica", new PanelGenerica(new PilaGenerica()));
        Agregar(tabs, "Cola genérica", new PanelGenerica(new ColaGenerica()));
        Agregar(tabs, "Lista genérica", new PanelGenerica(new ListaGenerica()));
        Agregar(tabs, "Estático vs dinámico", new PanelComparacion());
        layout.Controls.Add(tabs, 0, 1);
        layout.Controls.Add(new Label
        {
            Text = "Jesús Armando Zendejo Altamirano | Estructuras de Datos | Unidad 3",
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter
        }, 0, 2);
        Controls.Add(layout);
    }

    private static void Agregar(TabControl tabs, string nombre, UserControl contenido)
    {
        var tab = new TabPage(nombre) { BackColor = Color.White };
        tab.Controls.Add(contenido);
        tabs.TabPages.Add(tab);
    }
}

// Base gráfica compartida; los datos siguen viviendo en los modelos.
internal abstract class PanelBase : UserControl
{
    protected readonly FlowLayoutPanel Acciones = new FlowLayoutPanel();
    protected readonly TextBox Entrada = new TextBox { Width = 100 };
    protected readonly Label Estado = new Label { Dock = DockStyle.Fill };
    protected readonly DataGridView Tabla = new DataGridView();
    private readonly Label mensaje = new Label { Dock = DockStyle.Fill };
    private readonly TextBox registro = new TextBox();
    private int paso;

    protected PanelBase(string ayuda)
    {
        Dock = DockStyle.Fill; Padding = new Padding(12);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 7 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 78));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 106));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        layout.Controls.Add(new Label { Text = ayuda, Dock = DockStyle.Fill }, 0, 0);
        Acciones.Dock = DockStyle.Fill; Acciones.WrapContents = true;
        Acciones.Controls.Add(new Label { Text = "Valor entero:", AutoSize = true, Margin = new Padding(0, 8, 5, 0) });
        Entrada.AccessibleName = "Número entero para la operación";
        Acciones.Controls.Add(Entrada);
        layout.Controls.Add(Acciones, 0, 1);
        Estado.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        layout.Controls.Add(Estado, 0, 2);
        Tabla.Dock = DockStyle.Fill;
        Tabla.ReadOnly = true; Tabla.AllowUserToAddRows = false;
        Tabla.AllowUserToDeleteRows = false; Tabla.AllowUserToResizeRows = false;
        Tabla.RowHeadersVisible = false;
        Tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        Tabla.BackgroundColor = Color.White; Tabla.RowTemplate.Height = 29;
        layout.Controls.Add(Tabla, 0, 3);
        layout.Controls.Add(mensaje, 0, 4);
        registro.Dock = DockStyle.Fill; registro.Multiline = true; registro.ReadOnly = true;
        registro.ScrollBars = ScrollBars.Vertical; registro.Font = new Font("Consolas", 9F);
        layout.Controls.Add(registro, 0, 5);
        var herramientas = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
        Boton(herramientas, "Cargar demostración", Demostrar);
        Boton(herramientas, "Guardar captura PNG", Capturar);
        Boton(herramientas, "Guardar registro TXT", GuardarRegistro);
        layout.Controls.Add(herramientas, 0, 6);
        Controls.Add(layout);
        mensaje.Text = "Listo. Las posiciones se numeran desde cero.";
    }

    protected abstract void Refrescar();
    protected abstract void Demostrar();
    protected int LeerEntero()
    {
        if (!int.TryParse(Entrada.Text, out int valor))
            throw new ArgumentException("Escribe un número entero de 32 bits.");
        return valor;
    }
    protected void Boton(string texto, Func<string> accion) =>
        Boton(Acciones, texto, () => Ejecutar(accion));
    protected static void Boton(FlowLayoutPanel panel, string texto, Action accion)
    {
        var boton = new Button { Text = texto, AutoSize = true, Height = 33, Padding = new Padding(5, 0, 5, 0) };
        boton.Click += (_, _) => accion(); panel.Controls.Add(boton);
    }
    protected void Columnas(params string[] nombres)
    {
        foreach (string nombre in nombres)
        {
            int indice = Tabla.Columns.Add("Col" + Tabla.Columns.Count, nombre);
            Tabla.Columns[indice].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }
    protected void Ejecutar(Func<string> accion)
    {
        try { Informar(accion(), false); }
        catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
        { Informar(ex.Message, true); }
        Refrescar();
    }
    protected void ReiniciarRegistro() { registro.Clear(); paso = 0; }
    protected void Informar(string texto, bool error)
    {
        mensaje.ForeColor = error ? Color.DarkRed : Color.DarkGreen; mensaje.Text = texto;
        if (registro.TextLength > 18000) registro.Clear();
        registro.AppendText($"{++paso:00}. {texto}\r\n");
    }
    private void Capturar()
    {
        using var dialogo = new SaveFileDialog { Filter = "Imagen PNG|*.png", FileName = "Tarea32_captura.png" };
        if (dialogo.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            Form form = FindForm()!; form.Refresh();
            using var imagen = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(imagen, new Rectangle(Point.Empty, form.Size));
            imagen.Save(dialogo.FileName, ImageFormat.Png);
            Informar("Captura guardada.", false);
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is System.Runtime.InteropServices.ExternalException)
        { Informar("No se pudo guardar la captura: " + ex.Message, true); }
    }
    private void GuardarRegistro()
    {
        using var dialogo = new SaveFileDialog { Filter = "Texto|*.txt", FileName = "Tarea32_registro.txt" };
        if (dialogo.ShowDialog(this) != DialogResult.OK) return;
        try { File.WriteAllText(dialogo.FileName, Estado.Text + "\r\n\r\n" + registro.Text); Informar("Registro guardado.", false); }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        { Informar("No se pudo guardar el registro: " + ex.Message, true); }
    }
}

internal sealed class PanelEnlazada : PanelBase
{
    private readonly ListaEnlazada<int> lista = new ListaEnlazada<int>();
    private readonly NumericUpDown posicion = new NumericUpDown { Width = 70, Minimum = 0, Maximum = 1000000 };

    public PanelEnlazada() : base("Lista propia con nodos: cada nodo contiene un valor y una referencia al siguiente.\nLa tabla muestra posiciones lógicas, no direcciones reales de memoria.")
    {
        Acciones.Controls.Add(new Label { Text = "Posición:", AutoSize = true, Margin = new Padding(5, 8, 5, 0) });
        Acciones.Controls.Add(posicion);
        Boton("Agregar al final", () => { int n = LeerEntero(); lista.Agregar(n); return "Agregado " + n + " al final."; });
        Boton("Insertar", () => { int n = LeerEntero(); lista.Insertar((int)posicion.Value, n); return $"Insertado {n} en posición {posicion.Value}."; });
        Boton("Eliminar posición", () => $"Eliminado {lista.Eliminar((int)posicion.Value)} de la posición {posicion.Value}.");
        Boton("Buscar valor", () => { int n = LeerEntero(); int i = lista.Buscar(n); return i < 0 ? $"No se encontró {n}." : $"{n} encontrado en posición {i}."; });
        Boton("Limpiar", () => { lista.Limpiar(); return "Lista vacía."; });
        Columnas("Posición lógica", "Valor del nodo", "Enlace siguiente", "Referencia de entrada");
        Refrescar();
    }
    protected override void Refrescar()
    {
        int[] valores = lista.AArreglo(); Tabla.Rows.Clear();
        for (int i = 0; i < valores.Length; i++)
            Tabla.Rows.Add(i, valores[i], i + 1 < valores.Length ? "Nodo en posición " + (i + 1) : "null (fin)",
                (i == 0 ? "cabeza " : "") + (i == valores.Length - 1 ? "cola" : ""));
        Estado.Text = $"Cantidad de nodos: {lista.Cantidad}   |   Vacía: {lista.EstaVacia}\nCada fila representa un nodo; el último enlace termina en null.";
        Tabla.ClearSelection();
    }
    protected override void Demostrar()
    {
        lista.Limpiar(); ReiniciarRegistro();
        Ejecutar(() => { lista.Agregar(10); lista.Agregar(20); lista.Agregar(30); return "Agregados 10, 20 y 30."; });
        Ejecutar(() => { lista.Insertar(1, 15); return "Insertado 15 en posición 1: [10, 15, 20, 30]."; });
        Ejecutar(() => "20 encontrado en posición " + lista.Buscar(20) + ".");
        Ejecutar(() => "Eliminado " + lista.Eliminar(2) + " de posición 2. Quedan [10, 15, 30].");
        posicion.Value = 1;
    }
}

internal sealed class PanelGenerica : PanelBase
{
    private readonly IColeccionDinamica coleccion;
    public PanelGenerica(IColeccionDinamica coleccion) : base(coleccion.Explicacion)
    {
        this.coleccion = coleccion;
        Boton("Agregar", () => { int n = LeerEntero(); coleccion.Agregar(n); return "Agregado " + n + "."; });
        Boton("Retirar", () => "Sale " + coleccion.Retirar() + ".");
        Boton("Consultar", () => "Siguiente: " + coleccion.Consultar() + "; la cantidad no cambia.");
        Boton("Limpiar", () => { coleccion.Limpiar(); return "Colección vacía."; });
        if (coleccion is ListaGenerica lista)
            Boton("Compactar", () => { lista.Compactar(); return "Reserva ajustada mediante TrimExcess."; });
        Columnas("Posición en la vista", "Valor", "Orden de lectura"); Refrescar();
    }
    protected override void Refrescar()
    {
        int[] datos = coleccion.Valores(); Tabla.Rows.Clear();
        for (int i = 0; i < datos.Length; i++)
            Tabla.Rows.Add(i, datos[i], i == 0 ? "Siguiente en retirar o consultar" : "");
        Estado.Text = $"Cantidad: {coleccion.Cantidad}   |   Vacía: {coleccion.Cantidad == 0}\n{coleccion.Detalle}";
        Tabla.ClearSelection();
    }
    protected override void Demostrar()
    {
        coleccion.Limpiar(); ReiniciarRegistro();
        int limite = coleccion is ListaGenerica ? 7 : 3;
        for (int i = 1; i <= limite; i++)
        {
            int n = i * 10;
            Ejecutar(() => { coleccion.Agregar(n); return "Agregado " + n + "."; });
        }
        // List muestra siete elementos para comparar Count con la capacidad interna.
        if (coleccion is not ListaGenerica)
            Ejecutar(() => "Sale " + coleccion.Retirar() + ".");
    }
}

internal sealed class PanelComparacion : PanelBase
{
    private readonly ComparacionMemoria modelo = new ComparacionMemoria(5);
    public PanelComparacion() : base("Se intenta agregar cada valor a un arreglo fijo de 5 posiciones y a una lista enlazada propia.\nLos contadores describen posiciones y nodos; no son mediciones de bytes de memoria.")
    {
        Boton("Agregar a ambos", () => Agregar(LeerEntero()));
        Boton("Limpiar ambos", () => { modelo.Limpiar(); return "Ambas estructuras vacías."; });
        Columnas("Posición lógica", "Arreglo fijo", "Lista enlazada dinámica"); Refrescar();
    }
    private string Agregar(int n)
    {
        bool aceptado = modelo.Agregar(n);
        return aceptado ? $"{n}: aceptado en ambas estructuras." : $"{n}: arreglo lleno; aceptado únicamente en la lista enlazada.";
    }
    protected override void Refrescar()
    {
        int[] fijo = modelo.ValoresFijos(); int[] dinamico = modelo.ValoresDinamicos();
        Tabla.Rows.Clear();
        for (int i = 0; i < Math.Max(modelo.CapacidadFija, dinamico.Length); i++)
            Tabla.Rows.Add(i, i < fijo.Length ? fijo[i].ToString() : i < modelo.CapacidadFija ? "Libre" : "Sin posición reservada",
                i < dinamico.Length ? dinamico[i].ToString() : "Sin nodo");
        Estado.Text = $"Arreglo: {modelo.CantidadFija}/{modelo.CapacidadFija}; espacios sin usar: {modelo.EspaciosSinUsar}\n" +
            $"Lista enlazada: {modelo.CantidadDinamica} nodos y {modelo.CantidadDinamica} campos de enlace siguiente (incluye el null final).";
        Tabla.ClearSelection();
    }
    protected override void Demostrar()
    {
        modelo.Limpiar(); ReiniciarRegistro();
        for (int i = 1; i <= 8; i++) { int n = i * 10; Ejecutar(() => Agregar(n)); }
    }
}
