using System;

class Buscaminas
{
    static char[] tablero;
    static bool[] revelado;
    static Random rand = new Random();
    static int nivel = 1;
    static int tamano;
    static int bombas;
    static int bombasEncontradas;

    static void Main()
    {
        do
        {
            DefinirNivel();
            Console.WriteLine($"Nivel {nivel} - Tamaño: {tamano} - Bombas: {bombas}");
            IniciarTablero();
            Jugar();
            MostrarTableroCompleto();
            Console.WriteLine("¿Quieres continuar al siguiente nivel? (s/n)");
            if (Console.ReadLine().ToLower() != "s")
                break;
            nivel++;
            if (nivel > 3) nivel = 1;  // Reiniciar al nivel 1 después del nivel 3
        } while (true);
    }

    static void DefinirNivel()
    {
        if (nivel == 1) { tamano = 10; bombas = 1; }
        else if (nivel == 2) { tamano = 15; bombas = 2; }
        else { tamano = 20; bombas = 3; }

        tablero = new char[tamano];
        revelado = new bool[tamano];
        bombasEncontradas = 0;
    }

    static void IniciarTablero()
    {
        for (int i = 0; i < tamano; i++)
        {
            tablero[i] = '0';
            revelado[i] = false;
        }

        int[] posiciones = new int[bombas];
        for (int i = 0; i < bombas; i++)
        {
            int pos;
            do
            {
                pos = rand.Next(tamano);
                // Verificamos que las bombas no estén demasiado cerca
            } while (tablero[pos] == '*' || (i > 0 && Math.Abs(pos - posiciones[i - 1]) < 4));
            posiciones[i] = pos;
            tablero[pos] = '*';  // Coloca la bomba
            MarcarCercania(pos); // Marca las celdas cercanas
        }
    }

    static void MarcarCercania(int pos)
    {
        for (int i = Math.Max(0, pos - 2); i <= Math.Min(tamano - 1, pos + 2); i++)
        {
            if (tablero[i] != '*')  // No marcar las celdas con bombas
            {
                int distancia = Math.Abs(pos - i);
                tablero[i] = (char)(distancia + '0');  // Asigna la proximidad
            }
        }
    }

    static void Jugar()
    {
        while (true)
        {
            MostrarTablero();
            Console.Write("Ingresa una posición (1 - {0}): ", tamano);
            int pos = int.Parse(Console.ReadLine()) - 1;

            if (tablero[pos] == '*')
            {
                Console.WriteLine("¡Boom! Encontraste una bomba.");
                revelado[pos] = true;
                bombasEncontradas++;
                MostrarTablero();
                if (nivel == 1 || bombasEncontradas == bombas)
                {
                    Console.WriteLine("¡Nivel completado! Pasas al siguiente nivel.");
                    break;
                }
            }
            else
            {
                revelado[pos] = true;
                if (VerificarVictoria())
                {
                    Console.WriteLine("¡Felicidades! Has limpiado el tablero. Pasas al siguiente nivel.");
                    break;
                }
            }
        }
    }

    static void MostrarTablero()
    {
        Console.Clear();
        for (int i = 0; i < tamano; i++)
        {
            if (revelado[i])
                Console.Write(tablero[i] + " ");
            else
                Console.Write("# ");  // Celdas no reveladas mostradas como '#'
        }
        Console.WriteLine();
    }

    static void MostrarTableroCompleto()
    {
        Console.WriteLine("Tablero completo:");
        for (int i = 0; i < tamano; i++)
        {
            Console.Write(tablero[i] + " ");
        }
        Console.WriteLine();
    }

    static bool VerificarVictoria()
    {
        int celdasNoBombas = tamano - bombas;
        int descubiertas = 0;
        for (int i = 0; i < tamano; i++)
            if (revelado[i] && tablero[i] != '*')
                descubiertas++;
        return descubiertas == celdasNoBombas;
    }
}
