CREATE TABLE Customers(
	Id int primary key,
	FirstName varchar(100) not null,
	LastName varchar(100) not null,
	BuyAmount int,
	CreatedAt timestamp not null,
	UpdatedAt timestamp not null,
	Password varchar(256) not null
);

CREATE TABLE teste(
	Id int primary key auto_increment,
	elapsed timestamp not null
);

drop table if exists RequestTrace;
create table RequestTrace(
	id varchar(36) primary key,
	ClientCode varchar(36) not null,
	HttpMethod varchar(20) not null,
	Address varchar(256) not null,
	HttpStatusCode int not null,
	ElapsedTime time(6) not null,
	CreatedAt timestamp not null,
	UpdatedAt timestamp not null
);

insert into RequestTrace 
(id, ClientCode,HttpMethod,Address,Request,HttpStatusCode,Response,
ElapsedTime, CreatedAt, UpdatedAt) values 
(uuid(),'...', '...', '...', '...', 2, '...', '101212.123456', now(), now());
