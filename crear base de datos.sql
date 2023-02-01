CREATE DATABASE Estudiantes;

Use Estudiantes;

create table Estado(
id int identity(1,1) primary key,
Nombre varchar(100)
);

create table Asistencia(
id int identity(1,1) primary key,
nomAsis varchar(50) not null
);

create table Jornada(
id int identity (1,1) primary key,
jornada varchar (50) not null
);

create table Estudiante(
id int identity(1,1) primary key,
nombre varchar(100) not null,
apellido varchar(100) not null,
telefono varchar(100) not null,
direccion varchar(50) not null,
jornada varchar (50) not null,
correo varchar (50) not null,
contraseña varchar(50) not null,
imagenURL nvarchar (500),
idEstado int,
idAsistencia int,
idJornadaI int,
constraint fk_idJornadaI foreign key (idJornadaI) references Jornada(id),
constraint fk_idEstado foreign key(idEstado) references Estado(id),
constraint fk_idAsistencia foreign key(idAsistencia) references Asistencia(id)
);

create table Materia(
id int identity(1,1) primary key,
nombreMateria varchar(50) not null
);

create table Nota(
id int identity(1,1) primary key,
calificacion varchar(50) not null,
idMateria int,
idEstudiante int,
constraint idEstudiante foreign key(idEstudiante) references Estudiante(id),
constraint fk_idMateria foreign key(idMateria) references Materia(id)
);

create table Instructor(
id int identity(1,1) primary key,
nombreInstructor varchar(50) not null,
direccion varchar (50) not null,
matreria varchar (50) not null,
telefono varchar (50) not null,
idJornada int,
idEstudianteI int,
idMateriaI int,
constraint fk_idEstudianteI foreign key(idEstudianteI) references Estudiante(id),
constraint fk_idJornada foreign key(idJornada) references Jornada(id),
constraint fk_idMateriaI foreign key (idMateriaI) references Materia(id)
);