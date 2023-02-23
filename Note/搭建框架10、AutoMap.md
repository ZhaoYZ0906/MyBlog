# 搭建框架10、AutoMap

### 1、Dto说明与安装包

```
Ø 什么是DTO?
　　数据传输对象（DTO)(DataTransfer Object)，是一种设计模式之间传输数据的软件应用系统。数据传输目标往往是数据访问对象从而从数据库中检索数据。数据传输对象与数据交互对象或数据访问对象之间的差异是一个以不具有任何行为除了存储和检索的数据（访问和存取器）。

Ø 为什么用？
　　它的目的只是为了对领域对象进行数据封装，实现层与层之间的数据传递。为何不能直接将领域对象用于数据传递？因为领域对象更注重领域，而DTO更注重数据。不仅如此，由于“富领域模型”的特点，这样做会直接将领域对象的行为暴露给表现层。

　　需要了解的是，数据传输对象DTO本身并不是业务对象。数据传输对象是根据UI的需求进行设计的，而不是根据领域对象进行设计的。比如，Customer领域对象可能会包含一些诸如FirstName, LastName, Email, Address等信息。但如果UI上不打算显示Address的信息，那么CustomerDTO中也无需包含这个 Address的数据”。

Ø 什么是领域对象？
　　领域模型就是面向对象的，面向对象的一个很重要的点就是：“把事情交给最适合的类去做”，即：“你得在一个个领域类之间跳转，才能找出他们如何交互”。在我们的系统中Model（EF中的实体）就是领域模型对象。领域对象主要是面对业务的，我们是通过业务来定义Model的。
```

注：可以简单的理解为Dto只负责展示，而model可以体现业务逻辑

需要安装：AutoMapper、AutoMapper.Extensions.Microsoft.DependencyInje两个包

### 2、基本配置及使用方法

2.1 编写配置文件

```
// 配置文件负责配置转换逻辑，各种配置方法请参考Note中关于automap的笔记或官方文档
public class AllProfile : Profile
    {
        public AllProfile()
        {
            CreateMap<BlogArticle, BlogDto>().ReverseMap();
        }
    }
```

2.2 注入服务及使用

```
// ConfigureServices中注入
services.AddAutoMapper(typeof(AllProfile));

// 使用方式,一般在服务层中使用
		IMapper IMapper;
        public LoginController(IMapper IMapper)
        {
            this.IMapper = IMapper;
        }

        [HttpGet]
        [Route("Token")]
        public async Task<object> GetJwtStr(string name, string pass)
        {
            BlogArticle cs = new BlogArticle() {bID=1,btitle="12313135456" };
            BlogDto BlogDto = IMapper.Map<BlogDto>(cs);
		}
```

### 3、文档地址

[配置 — 自动映射器文档 (automapper.org)](https://docs.automapper.org/en/latest/Configuration.html)