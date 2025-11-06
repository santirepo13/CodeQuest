# CodeQuest: Plataforma de evaluación gamificada en C# para Windows

Informe preparado para presentación académica en formato APA 7.ª edición (versión Markdown para conversión a PDF).

Institución: ______________________________________
Asignatura: Herramientas de Programación II
Docente: _________________________________________
Autor(es): _______________________________________
Fecha: 6 de noviembre de 2025

## Resumen

CodeQuest es una aplicación de escritorio para Windows, desarrollada en C# y .NET, que integra principios de gamificación para reforzar el aprendizaje de fundamentos de programación. La solución permite a personas estudiantes practicar y autoevaluar sus conocimientos mediante rondas de preguntas con dificultad progresiva, registrar su progreso mediante puntos de experiencia (XP) y niveles, y comparar desempeño en un ranking. A nivel técnico, el proyecto implementa una arquitectura en capas con patrones de diseño contemporáneos (Repositorio, Servicio, Fábrica y Singleton de conexión) y un backend soportado en SQL Server LocalDB con procedimientos almacenados para garantizar integridad, consistencia y trazabilidad de datos. La interfaz, construida con Windows Forms, ofrece una experiencia guiada para usuarios y un panel de administración para gestionar preguntas, opciones y estadísticas. Este informe presenta la introducción, los objetivos y la descripción general del proyecto, situando su pertinencia pedagógica y su aporte tecnológico.

Palabras clave: gamificación; C#; .NET; Windows Forms; SQL Server; arquitectura por capas; patrones de diseño; evaluación formativa.

## 1. Introducción

En entornos de formación en programación, la práctica constante y la retroalimentación inmediata son factores determinantes para la consolidación del aprendizaje. Sin embargo, lograr que las y los estudiantes mantengan la motivación y un hábito de evaluación continua suele ser desafiante sin estrategias lúdicas y métricas claras de progreso.

CodeQuest surge como respuesta a esa necesidad, proponiendo una experiencia de cuestionarios de opción múltiple cuidadosamente estructurada y gamificada. La aplicación organiza el ejercicio en rondas con dificultad incremental y tiempos de respuesta, otorga XP por aciertos y bonificaciones por desempeño ágil, y sintetiza los resultados en un ranking que estimula la mejora.

Desde el punto de vista tecnológico, CodeQuest materializa buenas prácticas de ingeniería de software en un contexto académico: separación clara de responsabilidades (UI, servicios, repositorios y base de datos), uso de patrones de diseño para desacoplar capas y facilitar pruebas, y un modelo de datos normalizado que respalda la calidad de la información.

En este documento se exponen los objetivos que guiaron el desarrollo y la descripción general del producto, abarcando su propósito pedagógico, principales funcionalidades y decisiones arquitectónicas.

## 2. Objetivos

### 2.1 Objetivo general

Diseñar e implementar una aplicación de escritorio en C# que, mediante mecánicas de gamificación y una arquitectura por capas, facilite la práctica, evaluación y seguimiento del aprendizaje de fundamentos de programación.

### 2.2 Objetivos específicos

- Integrar un flujo de juego basado en 3 rondas de 3 preguntas cada una, con dificultad progresiva y medición de tiempo de respuesta.
- Implementar un sistema de puntuación y XP con bonificaciones por desempeño (tiempos totales ≤30 s y ≤60 s) y cálculo de niveles.
- Desarrollar un módulo de ranking que ordene a las personas usuarias por XP y estadísticas de juego relevantes.
- Proveer un panel de administración para la gestión de preguntas y opciones (crear, actualizar, eliminar, marcar respuesta correcta).
- Diseñar una arquitectura en capas con patrones Repositorio, Servicio, Fábrica e implementación Singleton para la conexión a datos.
- Persistir la información en SQL Server LocalDB mediante tablas normalizadas y procedimientos almacenados que aseguren integridad y consistencia.
- Implementar manejo robusto de errores y mensajes de retroalimentación en la UI para favorecer la usabilidad y la recuperación ante fallos.
- Documentar las decisiones técnicas clave y el modelo de datos para facilitar mantenimiento y extensibilidad futura.

## 3. Descripción general del proyecto

### 3.1 Visión del producto

CodeQuest es un entorno de práctica gamificada orientado a reforzar, de manera incremental y divertida, conceptos fundamentales de programación (sintaxis, estructuras de control, tipos de datos, orientación a objetos, patrones y buenas prácticas). Su diseño prioriza la simplicidad operativa y la retroalimentación inmediata.

### 3.2 Usuarios y actores

- Estudiante/jugador: accede al juego, responde preguntas, recibe XP y consulta su posición en el ranking.
- Administrador/a: gestiona preguntas y opciones, valida respuestas correctas y puede realizar tareas de mantenimiento sobre usuarios y estadísticas.

### 3.3 Funcionalidades clave

- Juego por rondas: tres rondas consecutivas, cada una con tres preguntas de opción múltiple. La dificultad incrementa de fácil a medio y difícil.
- Puntuación y XP: 10 XP por respuesta correcta, bonificaciones por velocidad y cálculo de nivel cada 100 XP acumulados.
- Registro y cierre de ronda: el sistema consolida aciertos, duración total y puntaje, y actualiza la experiencia del usuario.
- Ranking: listado de las mejores puntuaciones con indicadores como XP, nivel, rondas jugadas y promedio de puntaje.
- Administración: alta, edición y baja de preguntas y opciones; marcación de la opción correcta y reordenamiento aleatorio en la presentación.

### 3.4 Arquitectura técnica

La solución adopta una arquitectura por capas:
- Capa de presentación (Windows Forms): orquesta la experiencia de usuario y captura eventos de interacción.
- Capa de servicios (lógica de negocio): coordina flujos del juego, reglas de puntuación y acceso a datos mediante interfaces desacopladas.
- Capa de repositorios (acceso a datos): encapsula consultas y operaciones sobre la base de datos a través de repositorios especializados.
- Capa de datos (SQL Server): esquema normalizado con tablas para usuarios, rondas, preguntas, opciones y respuestas; procedimientos almacenados para operaciones atómicas, consistentes y transaccionales.

Entre los patrones aplicados destacan: Repositorio (aislamiento del acceso a datos), Servicio (coordinación de reglas y orquestación), Fábrica (obtención centralizada de dependencias) y Singleton para la administración de la cadena de conexión. Este diseño favorece cohesión, bajo acoplamiento, testabilidad y mantenimiento.

### 3.5 Modelo de datos y persistencia

El modelo define entidades para Usuarios, Rondas, Preguntas, Opciones y Respuestas de Ronda. Las restricciones de unicidad, claves foráneas y procedimientos almacenados aseguran integridad referencial y evitan inconsistencias (por ejemplo, asegurando que cada pregunta tenga exactamente una respuesta marcada como correcta en un conjunto).

Las operaciones críticas (inicio de ronda, registro de respuesta, cierre de ronda, gestión de opciones y preguntas) se ejecutan mediante procedimientos almacenados transaccionales, lo que permite consolidar puntajes, XP, bonificaciones y tiempos de forma confiable y auditable.

### 3.6 Interfaz de usuario

La UI presenta pantallas para inicio/selección, juego por rondas, resultados finales, información de mecánicas y ranking. El panel administrativo permite gestionar preguntas y opciones de forma guiada, con retroalimentación y validaciones básicas. El diseño visual es sobrio, claro y consistente con aplicaciones de escritorio educativas.

### 3.7 Mecanismos de calidad y seguridad

- Manejo de errores con mensajes contextualizados en la inicialización y en operaciones de datos, fomentando la recuperación segura ante fallos (por ejemplo, indisponibilidad de la base de datos).
- Validaciones de entrada mínimas en la capa de presentación y de negocio para evitar estados inválidos.
- Separación de responsabilidades y uso de procedimientos almacenados para reducir superficie de errores y mejorar la trazabilidad de cambios.

### 3.8 Supuestos y limitaciones

- El entorno de ejecución es Windows 10/11 con .NET 8 y acceso local a SQL Server (LocalDB o Express).
- El alcance actual se centra en preguntas de opción múltiple para fundamentos de programación; no incluye, por ahora, preguntas abiertas ni ejecución de código en línea.
- La autenticación de administradores es básica y se asume el uso en un entorno controlado (aula/laboratorio). Puede endurecerse en versiones posteriores.
- La interfaz está optimizada para resoluciones estándar de escritorio; no se contemplan aún adaptaciones móviles.

### 3.9 Criterios de éxito y métricas

- Estabilidad de la aplicación en sesiones de juego completas (3 rondas) sin errores no controlados.
- Persistencia correcta de resultados, cálculo de XP/bonificaciones y actualización de niveles.
- Facilidad de uso percibida (tiempo de aprendizaje < 5 minutos) y satisfacción general > 80% en encuesta breve.
- Capacidad del panel administrativo para mantener un banco de preguntas vigente y balanceado por dificultad.


