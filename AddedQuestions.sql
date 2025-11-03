EXEC dbo.spQuestion_New @Text = N'¿Qué palabra clave se usa para mostrar un mensaje por consola en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=1, @ChoiceText=N'Console.WriteLine()', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=1, @ChoiceText=N'print', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=1, @ChoiceText=N'echo', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=1, @ChoiceText=N'display', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Qué tipo de dato almacena números enteros en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=2, @ChoiceText=N'int', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=2, @ChoiceText=N'string', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=2, @ChoiceText=N'bool', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=2, @ChoiceText=N'double', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Qué valor lógico representa falso en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=3, @ChoiceText=N'false', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=3, @ChoiceText=N'0', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=3, @ChoiceText=N'null', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=3, @ChoiceText=N'none', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Cuál es el operador de asignación en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=4, @ChoiceText=N'=', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=4, @ChoiceText=N':', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=4, @ChoiceText=N'==', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=4, @ChoiceText=N'->', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Cuál es el tipo de dato para texto en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=5, @ChoiceText=N'string', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=5, @ChoiceText=N'char', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=5, @ChoiceText=N'text', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=5, @ChoiceText=N'varchar', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Qué palabra clave se usa para definir una condición en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=6, @ChoiceText=N'if', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=6, @ChoiceText=N'when', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=6, @ChoiceText=N'while', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=6, @ChoiceText=N'then', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Qué palabra clave define un bucle en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=7, @ChoiceText=N'for', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=7, @ChoiceText=N'loop', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=7, @ChoiceText=N'repeat', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=7, @ChoiceText=N'do', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Cómo se comenta una sola línea en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=8, @ChoiceText=N'// comentario', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=8, @ChoiceText=N'# comentario', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=8, @ChoiceText=N'-- comentario', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=8, @ChoiceText=N'/* comentario */', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Qué palabra clave define una clase en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=9, @ChoiceText=N'class', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=9, @ChoiceText=N'object', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=9, @ChoiceText=N'struct', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=9, @ChoiceText=N'type', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Qué símbolo termina una instrucción en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=10, @ChoiceText=N';', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=10, @ChoiceText=N'.', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=10, @ChoiceText=N':', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=10, @ChoiceText=N',', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Qué palabra clave inicia el método principal en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=11, @ChoiceText=N'main', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=11, @ChoiceText=N'principal', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=11, @ChoiceText=N'start', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=11, @ChoiceText=N'void', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Cómo se llama el entorno oficial para desarrollar en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=12, @ChoiceText=N'Visual Studio', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=12, @ChoiceText=N'Eclipse', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=12, @ChoiceText=N'PyCharm', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=12, @ChoiceText=N'IntelliJ', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Qué tipo de datos almacena valores verdadero/falso en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=13, @ChoiceText=N'bool', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=13, @ChoiceText=N'int', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=13, @ChoiceText=N'string', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=13, @ChoiceText=N'float', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Qué símbolo se utiliza para concatenar en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=14, @ChoiceText=N'+', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=14, @ChoiceText=N'&', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=14, @ChoiceText=N'%', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=14, @ChoiceText=N'$', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Con qué palabra se define un método en C#?', @Difficulty = 1;
EXEC dbo.spOption_new @QuestionID=15, @ChoiceText=N'void', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=15, @ChoiceText=N'def', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=15, @ChoiceText=N'function', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=15, @ChoiceText=N'sub', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Con qué palabra reservada defines un arreglo en C#?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=16, @ChoiceText=N'new', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=16, @ChoiceText=N'array', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=16, @ChoiceText=N'list', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=16, @ChoiceText=N'collection', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Cuál es el valor por defecto de un int declarado pero no inicializado en una clase?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=17, @ChoiceText=N'0', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=17, @ChoiceText=N'null', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=17, @ChoiceText=N'none', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=17, @ChoiceText=N'NaN', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Qué operador se usa para comparar igualdad de valor en C#?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=18, @ChoiceText=N'==', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=18, @ChoiceText=N'=', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=18, @ChoiceText=N'===', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=18, @ChoiceText=N'equals', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Cómo se inicia un ciclo infinito usando while en C#?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=19, @ChoiceText=N'while(true)', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=19, @ChoiceText=N'while(1)', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=19, @ChoiceText=N'while()', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=19, @ChoiceText=N'while(false)', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Qué acceso tiene un campo marcado como private en C#?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=20, @ChoiceText=N'Sólo accesible dentro de la propia clase', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=20, @ChoiceText=N'Accesible desde otras clases', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=20, @ChoiceText=N'Accesible sólo en el paquete', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=20, @ChoiceText=N'Accesible desde cualquier parte', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Cuál es la palabra clave para heredar de una clase base en C#?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=21, @ChoiceText=N':', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=21, @ChoiceText=N'extends', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=21, @ChoiceText=N'inherits', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=21, @ChoiceText=N'base', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Cuál es el modificador de acceso por defecto para los miembros de una clase?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=22, @ChoiceText=N'private', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=22, @ChoiceText=N'public', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=22, @ChoiceText=N'internal', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=22, @ChoiceText=N'protected', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Qué palabra se usa para evitar que una clase sea heredada?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=23, @ChoiceText=N'sealed', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=23, @ChoiceText=N'static', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=23, @ChoiceText=N'final', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=23, @ChoiceText=N'const', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Cómo se llama al método que se ejecuta al crear una instancia de una clase?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=24, @ChoiceText=N'Constructor', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=24, @ChoiceText=N'Destructor', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=24, @ChoiceText=N'Starter', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=24, @ChoiceText=N'Main', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Cuál es el valor de retorno de un método que no devuelve nada?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=25, @ChoiceText=N'void', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=25, @ChoiceText=N'null', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=25, @ChoiceText=N'nothing', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=25, @ChoiceText=N'empty', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Con qué palabra clave lanzas una excepción en C#?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=26, @ChoiceText=N'throw', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=26, @ChoiceText=N'catch', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=26, @ChoiceText=N'error', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=26, @ChoiceText=N'raise', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Cuál es el método que convierte un string a entero en C#?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=27, @ChoiceText=N'Convert.ToInt32()', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=27, @ChoiceText=N'ToInteger()', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=27, @ChoiceText=N'stringToInt()', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=27, @ChoiceText=N'ValueOf()', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Qué palabra clave se usa para salir de un bucle anticipadamente en C#?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=28, @ChoiceText=N'break', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=28, @ChoiceText=N'continue', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=28, @ChoiceText=N'exit', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=28, @ChoiceText=N'stop', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Qué significa el modificador static en un método?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=29, @ChoiceText=N'Pertenece a la clase y no a una instancia', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=29, @ChoiceText=N'No puede ser accedido', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=29, @ChoiceText=N'Es abstracto', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=29, @ChoiceText=N'Se ejecuta automáticamente', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Quiénes pueden acceder a un campo protected?', @Difficulty = 2;
EXEC dbo.spOption_new @QuestionID=30, @ChoiceText=N'Solo la clase y sus derivadas', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=30, @ChoiceText=N'La misma clase solamente', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=30, @ChoiceText=N'Cualquier clase', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=30, @ChoiceText=N'Solo en el mismo archivo', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Qué interfaz garantiza que una clase tenga un método Dispose en C#?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=31, @ChoiceText=N'IDisposable', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=31, @ChoiceText=N'IEnumerable', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=31, @ChoiceText=N'IComparable', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=31, @ChoiceText=N'ICloneable', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Cuál es la palabra clave para declarar un método abstracto en C#?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=32, @ChoiceText=N'abstract', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=32, @ChoiceText=N'virtual', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=32, @ChoiceText=N'override', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=32, @ChoiceText=N'static', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'Si quieres asegurar que solo exista una instancia de una clase, ¿qué patrón usarías?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=33, @ChoiceText=N'Singleton', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=33, @ChoiceText=N'Factory', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=33, @ChoiceText=N'Strategy', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=33, @ChoiceText=N'Observer', @IsCorrect=0;

EXEC dbo.spQuestion_New @Text = N'¿Qué palabra clave se usa para indicar que un método puede ser sobreescrito en una subclase?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=34, @ChoiceText=N'virtual', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=34, @ChoiceText=N'abstract', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=34, @ChoiceText=N'static', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=34, @ChoiceText=N'new', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Qué método de las listas enlazadas permite agregar un elemento al final en C#?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=35, @ChoiceText=N'AddLast()', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=35, @ChoiceText=N'Append()', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=35, @ChoiceText=N'InsertEnd()', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=35, @ChoiceText=N'Push()', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Qué excepción lanza un acceso fuera de rango en un arreglo?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=36, @ChoiceText=N'IndexOutOfRangeException', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=36, @ChoiceText=N'ArgumentException', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=36, @ChoiceText=N'AccessViolationException', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=36, @ChoiceText=N'NullReferenceException', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Cómo se representa un valor nulo seguro en tipos de valor en C#?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=37, @ChoiceText=N'usando ?', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=37, @ChoiceText=N'using null', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=37, @ChoiceText=N'#nullable', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=37, @ChoiceText=N'(default)', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Qué palabra clave define una propiedad de sólo lectura?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=38, @ChoiceText=N'readonly', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=38, @ChoiceText=N'const', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=38, @ChoiceText=N'final', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=38, @ChoiceText=N'fixed', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Cuál es el resultado de 5 / 2 en C# usando enteros?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=39, @ChoiceText=N'2', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=39, @ChoiceText=N'2.5', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=39, @ChoiceText=N'3', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=39, @ChoiceText=N'Error', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Cómo se llama la clase base de todas las clases en C#?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=40, @ChoiceText=N'Object', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=40, @ChoiceText=N'BaseClass', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=40, @ChoiceText=N'Root', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=40, @ChoiceText=N'MainClass', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Qué operador se utiliza para el casting explícito en C#?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=41, @ChoiceText=N'()', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=41, @ChoiceText=N'as', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=41, @ChoiceText=N'is', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=41, @ChoiceText=N'cast()', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Qué ciclo es ideal cuando el número de iteraciones es desconocido?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=42, @ChoiceText=N'while', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=42, @ChoiceText=N'for', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=42, @ChoiceText=N'foreach', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=42, @ChoiceText=N'switch', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Cómo defines un método que puede recibir cualquier cantidad de argumentos?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=43, @ChoiceText=N'params', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=43, @ChoiceText=N'array', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=43, @ChoiceText=N'multiple', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=43, @ChoiceText=N'args', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Qué palabra clave se usa para implementar una interfaz en C#?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=44, @ChoiceText=N'interface', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=44, @ChoiceText=N'implements', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=44, @ChoiceText=N'with', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=44, @ChoiceText=N'supports', @IsCorrect=0;


EXEC dbo.spQuestion_New @Text = N'¿Qué hace el método ToString() en C#?', @Difficulty = 3;
EXEC dbo.spOption_new @QuestionID=45, @ChoiceText=N'Retorna la representación texto del objeto', @IsCorrect=1;
EXEC dbo.spOption_new @QuestionID=45, @ChoiceText=N'Devuelve el tipo de objeto', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=45, @ChoiceText=N'Convierte a entero', @IsCorrect=0;
EXEC dbo.spOption_new @QuestionID=45, @ChoiceText=N'Destruye el objeto', @IsCorrect=0;
