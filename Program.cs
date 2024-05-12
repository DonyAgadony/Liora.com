using System.Numerics;
using System.Net;
using static Project.Utils;
using System.Text.Json;
using System.Text;

namespace Project;
// FROM NOW ON IDO'S CODE ABOUT EQUATIONS OR SOMETHINGָָ
class EquationValues(double? X, double? Y, double? Z, double? W, double? N)
{
  public double? X = X;
  public double? Y = Y;
  public double? Z = Z;
  public double? W = W;
  public double? N = N;
}
class Equations(string? Eq1, string? Eq2, string? Eq3, string? Eq4)
{
  public string? Eq1 = Eq1;
  public string? Eq2 = Eq2;
  public string? Eq3 = Eq3;
  public string? Eq4 = Eq4;
}

class Program
{
  public static void Main()
  {
    /*───────────────────────────╮
    │ Creating the server object │
    ╰───────────────────────────*/
    var server = new HttpListener();
    server.Prefixes.Add("http://*:5000/");
    server.Start();

    Console.WriteLine("Server started. Listening for requests...");
    Console.WriteLine("Main page on http://localhost:5000/website/html/index.html");

    /*─────────────────────────╮
    │ Processing HTTP requests │
    ╰─────────────────────────*/
    while (true)
    {
      /*─────────────────────────────────────╮
      │ Creating the database context object │
      ╰─────────────────────────────────────*/
      var databaseContext = new DatabaseContext();

      /*────────────────────────────╮
      │ Waiting for an HTTP request │
      ╰────────────────────────────*/
      var serverContext = server.GetContext();
      var response = serverContext.Response;

      try
      {
        /*────────────────────────╮
        │ Handeling file requests │
        ╰────────────────────────*/
        serverContext.ServeFiles();

        /*───────────────────────────╮
        │ Handeling custome requests │
        ╰───────────────────────────*/
        HandleRequests(serverContext, databaseContext);

        /*───────────────────────────────╮
        │ Saving changes to the database │
        ╰───────────────────────────────*/
        databaseContext.SaveChanges();

      }
      catch (Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e);
        Console.ResetColor();
      }

      /*───────────────────────────────────╮
      │ Sending the response to the client │
      ╰───────────────────────────────────*/
      response.Close();
    }
  }

  public static void HandleRequests(HttpListenerContext serverContext, DatabaseContext databaseContext)
  {
    var request = serverContext.Request;
    var response = serverContext.Response;

    string absPath = request.Url!.AbsolutePath;
    if (absPath == "/addEquations")
    {
      Equations equations = request.GetBody<Equations>();
      EquationValues RtnToWeb = MainEquations(equations);
      Console.WriteLine("Equations Recieved");
      Console.WriteLine(equations.Eq1);
      Console.WriteLine(equations.Eq2);
      Console.WriteLine(equations.Eq3);
      Console.WriteLine(equations.Eq4);
      Console.WriteLine($"The slutions are: X = {RtnToWeb.X}, Y = {RtnToWeb.Y}, Z = {RtnToWeb.Z}, W = {RtnToWeb.W}");
      response.OutputStream.Write(ToBytes(RtnToWeb));
      Console.WriteLine("Sent back to website");
    }
  }
  public static byte[] ToBytes<T>(T value)
  {
    string json = JsonSerializer.Serialize(value);
    return Encoding.UTF8.GetBytes(json);
  }

  //From here it is idos code about equations.
  public static EquationValues MainEquations(Equations equations)
  {
    // example system of equations:
    // 2x - 8y + 1z - 9w = 43
    // -10x - 2y - 8z - 2w = 28
    // 8x - 8y + 2z + 5w = 13
    // -4x + 9y + 6z + 2w = -72
    string? FirEq = equations.Eq1;
    string? SecEq = equations.Eq2;
    string? ThiEq = equations.Eq3;
    string? FouEq = equations.Eq4;

    int num = 0;
    if (FirEq != null)
    {
      num = 1;
      if (SecEq != null)
      {
        num = 2;
        if (ThiEq != null)
        {
          num = 3;
          if (FouEq != null)
          {
            num = 4;
          }
        }
      }
    }

    EquationValues Eq1 = CreateEqInstance(FirEq, num);
    EquationValues Eq2 = CreateEqInstance(SecEq, num);
    EquationValues Eq3 = CreateEqInstance(ThiEq, num);
    EquationValues Eq4 = CreateEqInstance(FouEq, num);
    if (FirEq == null)
    {
      return new EquationValues(null, null, null, null, null);
    }
    if (SecEq == null && num == 1)
    {
      return new EquationValues(null, null, null, null, null);
    }
    if (ThiEq == null && num <= 2)
    {
      return new EquationValues(null, null, null, null, null);
    }
    if (FouEq == null && num <= 3)
    {
      return new EquationValues(null, null, null, null, null);
    }
    EquationValues[] EqSys = [Eq1, Eq2, Eq3, Eq4];
    EquationValues Sol = CalcByMatrix(EqSys);
    Console.WriteLine($"X = {Sol.X}");
    Console.WriteLine($"Y = {Sol.Y}");
    Console.WriteLine($"Z = {Sol.Z}");
    Console.WriteLine($"W = {Sol.W}");
    return Sol;
  }
  public static EquationValues CreateEqInstance(string? Eq, int variables)
  {
    EquationValues rtn = new(null, null, null, null, null);
    if (Eq == null)
      return new EquationValues(null, null, null, null, null);
    double x = 0;
    double y = 0;
    double z = 0;
    double w = 0;
    double n = 0;
    bool IsPostEq = false;
    bool IsNeg = false;
    for (int i = 0; i < Eq.Length; i++)
    {
      if (char.IsDigit(Eq[i]))
      {
        int StartSubIndex = i;
        while (char.IsDigit(Eq[i]) || Eq[i] == '.')
        {
          if (i == Eq.Length - 1)
            break;
          i++;
        }
        double num = 0;
        try
        {
          num = double.Parse(Eq[StartSubIndex..i]);
        }
        catch { }
        if (i == Eq.Length - 1)
        {
          if (char.IsDigit(Eq[i]))
          {
            int i1 = i + 1;
            num = double.Parse(Eq[StartSubIndex..i1]);
          }
        }
        switch (Eq[i])
        {
          case 'x':
            // -
            if (IsPostEq)
            {
              // -(+) = -
              if (!IsNeg)
                x -= num;
              // -(-) = +
              else
                x += num;
            }
            // +
            else
            {
              // +(+) = +
              if (!IsNeg)
                x += num;
              // +(-) = -
              else
                x -= num;
            }
            i++;
            break;
          case 'y':
            // -
            if (IsPostEq)
            {
              // -(+) = -
              if (!IsNeg)
                y -= num;
              // -(-) = +
              else
                y += num;
            }
            // +
            else
            {
              // +(+) = +
              if (!IsNeg)
                y += num;
              // +(-) = -
              else
                y -= num;
            }
            i++;
            break;
          case 'z':
            // -
            if (IsPostEq)
            {
              // -(+) = -
              if (!IsNeg)
                z -= num;

              // -(-) = +
              else
                z += num;
            }
            // +
            else
            {
              // +(+) = +
              if (!IsNeg)
                z += num;

              // +(-) = -
              else
                z -= num;
            }
            i++;
            break;
          case 'w':
            // - 
            if (IsPostEq)
            {
              // -(+) = -
              if (!IsNeg)
                w -= num;

              // -(-) = +
              else
                w += num;
            }
            // +
            else
            {
              // +(+) = +
              if (!IsNeg)
                w += num;

              // +(-) = -
              else
                w -= num;
            }
            i++;
            break;
          default:
            // +
            if (IsPostEq)
            {
              // +(+) = +
              if (!IsNeg)
              {
                n += num;
              }
              // +(-) = -
              else
              {
                n -= num;
              }
            }
            // -
            else
            {
              // -(+) = -
              if (!IsNeg)
                n -= num;

              // -(-) = +
              else
                n += num;
            }
            i++;
            break;
        }
      }
      try
      {
        switch (Eq[i])
        {
          case '=':
            IsPostEq = true;
            IsNeg = false;
            break;
          case '-':
            IsNeg = true;
            break;
          case '+':
            IsNeg = false;
            break;
        }
      }
      catch
      {
        switch (Eq[i - 1])
        {
          case '=':
            IsPostEq = true;
            IsNeg = false;
            break;
          case '-':
            IsNeg = true;
            break;
          case '+':
            IsNeg = false;
            break;
        }
      }
    }
    if (variables >= 1)
    {
      rtn.X = x;
      if (variables >= 2)
      {
        rtn.Y = y;
        if (variables >= 3)
        {
          rtn.Z = z;
          if (variables >= 4)
          {
            rtn.W = w;
          }
        }
      }
    }
    rtn.N = n;
    return rtn;
  }
  public static EquationValues CalcByMatrix(EquationValues[] Eq)
  {
    EquationValues EqVal = new(null, null, null, null, null);
    double? D = 0;
    double? Dx = 0;
    double? Dy = 0;
    double? Dz = 0;
    double? Dw = 0;
    double?[,] Matrix = {
            {Eq[0].X, Eq[0].Y, Eq[0].Z, Eq[0].W},
            {Eq[1].X, Eq[1].Y, Eq[1].Z, Eq[1].W},
            {Eq[2].X, Eq[2].Y, Eq[2].Z, Eq[2].W},
            {Eq[3].X, Eq[3].Y, Eq[3].Z, Eq[3].W}
        };

    if (Eq[0].W != null)
    {
      D = Calc4x4Matrix(Matrix);
      Dx = Calc4x4Matrix(ReplaceRow(Matrix, 0, Eq));
      Dy = Calc4x4Matrix(ReplaceRow(Matrix, 1, Eq));
      Dz = Calc4x4Matrix(ReplaceRow(Matrix, 2, Eq));
      Dw = Calc4x4Matrix(ReplaceRow(Matrix, 3, Eq));
      EqVal.X = Dx / D;
      EqVal.Y = Dy / D;
      EqVal.Z = Dz / D;
      EqVal.W = Dw / D;
    }
    else if (Eq[0].Z != null)
    {
      D = Calc3x3Matrix(Matrix);
      Console.WriteLine(D);
      Dx = Calc3x3Matrix(ReplaceRow(Matrix, 0, Eq));
      Console.WriteLine(Dx);
      Dy = Calc3x3Matrix(ReplaceRow(Matrix, 1, Eq));
      Console.WriteLine(Dy);
      Dz = Calc3x3Matrix(ReplaceRow(Matrix, 2, Eq));
      Console.WriteLine(Dz);
      EqVal.X = Dx / D;
      EqVal.Y = Dy / D;
      EqVal.Z = Dz / D;
    }
    else if (Eq[0].Y != null)
    {
      D = Calc2x2Matrix(Matrix);
      Dx = Calc2x2Matrix(ReplaceRow(Matrix, 0, Eq));
      Dy = Calc2x2Matrix(ReplaceRow(Matrix, 1, Eq));
      EqVal.X = Dx / D;
      EqVal.Y = Dy / D;
    }
    return EqVal;
  }
  public static double?[,] ReplaceRow(double?[,] Mat, int rep, EquationValues[] EqSys)
  {
    double?[,] RtnMatrix = new double?[4, 4];
    for (int i = 0; i < EqSys.Length; i++)
    {
      for (int j = 0; j < 4; j++)
      {
        RtnMatrix[i, j] = Mat[i, j];
      }
    }
    for (int i = 0; i < EqSys.Length; i++)
    {
      RtnMatrix[i, rep] = EqSys[i].N;
    }
    return RtnMatrix;
  }
  public static double? Calc2x2Matrix(double?[,] Mat)
  {
    return (Mat[0, 0] * Mat[1, 1]) - (Mat[0, 1] * Mat[1, 0]);
  }
  public static double?[,] NewGrid(double?[,] OldMatrix)
  {
    double?[,] Mat = new double?[3, 5];
    for (int i = 0; i < Mat.GetLength(1); i++)
    {
      for (int j = 0; j < Mat.GetLength(0); j++)
      {
        if (i < 3)
          Mat[j, i] = OldMatrix[j, i];
        else
        {
          Mat[j, i] = OldMatrix[j, i - 3];
        }
      }
    }
    return Mat;
  }
  public static double? Calc3x3Matrix(double?[,] Mat)
  {
    double? sum = 0;
    Mat = NewGrid(Mat);
    for (int i = 0; i < 3; i++)
    {
      sum += Mat[0, i] * Mat[1, i + 1] * Mat[2, i + 2];
      sum -= Mat[0, i + 2] * Mat[1, i + 1] * Mat[2, i];
    }
    return sum;
  }
  public static double? Calc4x4Matrix(double?[,] Mat)
  {
    Matrix4x4 Matrix = new(
     (float)Mat[0, 0]!, (float)Mat[0, 1]!, (float)Mat[0, 2]!, (float)Mat[0, 3]!,
     (float)Mat[1, 0]!, (float)Mat[1, 1]!, (float)Mat[1, 2]!, (float)Mat[1, 3]!,
     (float)Mat[2, 0]!, (float)Mat[2, 1]!, (float)Mat[2, 2]!, (float)Mat[2, 3]!,
     (float)Mat[3, 0]!, (float)Mat[3, 1]!, (float)Mat[3, 2]!, (float)Mat[3, 3]!);
    return Matrix.GetDeterminant();
  }

}

class DatabaseContext : DbContextWrapper
{
  public DatabaseContext() : base("Database") { }
}

