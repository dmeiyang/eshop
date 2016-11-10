create table aspnetusers
(
	Id char(32) primary key,
	Email varchar(50) null comment '用户邮箱',
	EmailConfirmed bit not null comment '是否认证邮箱',
	PasswordHash varchar(100) null comment '账户密码',
	SecurityStamp varchar(100) null comment '防伪印章',
	PhoneNumber varchar(100) null comment '用户手机',
	PhoneNumberConfirmed bit not null comment '是否认证手机',
	TwoFactorEnabled bit not null comment '是否启用双重身份验证',
	LockoutEndDateUtc datetime null comment '锁定结束时间',
	LockoutEnabled bit not null comment '是否启用锁定',
	AccessFailedCount int not null comment '登陆失败次数',
	UserName varchar(50) not null comment '用户名称',
	City varchar(50) null comment '所在城市',
	Type int default 0 not null comment '账户类型。0，用户；1，管理员',
	UpdateTime datetime not null comment '更新时间',
	CreateTime datetime not null comment '创建时间'
) comment '用户表';

create table aspnetuserclaims
(
	Id int auto_increment primary key,
	UserId char(32) not null comment '用户Id',
	ClaimType varchar(100) null comment 'ClaimType',
	ClaimValue varchar(100) null comment 'ClaimValue'
) comment 'claims表';

create table aspnetuserlogins
(
	UserId char(32) not null comment '',
	ProviderKey varchar(100) not null comment '',
	LoginProvider varchar(100) not null comment ''
) comment '登陆日志表';

create table aspnetuserroles
(
	UserId char(32) not null comment '',
	RoleId char(32) not null comment ''
) comment '用户角色表';

create table aspnetroles
(
	Id char(32) primary key,
	Name varchar(50) not null comment '角色名称',
	Discriminator varchar(50) null comment '区分IdentityRole和自定义Role',
	Description varchar(100) null comment '描述',
	CreateTime datetime not null comment '创建时间'
) comment '用户角色表';

create table product
(
	
) comment '商品表';

create table category
(

) comment '分类表';

create table brand
(

) comment '品牌表';

create table property
(
	Id char(32) primary key,
	DisplayName varchar(50) not null comment '显示名称，比如颜色、尺寸等',
	Name varchar(50) not null comment 'input中name属性值',
	Type int default 0 not null comment '属性类型（1、input；2、checkbox；3、radio；4、select；）',
	Content varchar(500) not null comment '属性内容，根据类型不同存储不同的值'
) comment '属性表';

create table standard
(
	Id char(32) primary key,
	DisplayName varchar(50) not null comment '显示名称，比如颜色、尺寸等',
	Name varchar(50) not null comment 'input中name属性值',
	Content varchar(100) not null comment '规格值，比如：红色,粉色等'
) comment '规格表';


