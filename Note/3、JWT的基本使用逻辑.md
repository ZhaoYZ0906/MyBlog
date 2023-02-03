## JWT的基本使用逻辑

[TOC]

### 1、jwt的基本定义

JSON Web Tokens，是一种基于JSON的、用于在网络上声明某种主张的令牌（token）。JWT通常由三部分组成: 头信息（header）, 消息体（payload）和签名（signature）。它是一种用于双方之间传递安全信息的表述性声明规范。JWT作为一个开放的标准（RFC 7519），定义了一种简洁的、自包含的方法，从而使通信双方实现以JSON对象的形式安全的传递信息。

总结：一种传输格式，一般用来在身份提供者和服务提供者间传递被认证的用户身份信息

### 2、jwt的在客户端、服务端、验证服务器之间的基本使用逻辑

![](D:\$备份\日常\MyBlog\Note\images\3-1.png)

### 3、生成一个token

```
public static string IssueJwt(string uid="1",role="admin")
        {
            // 声明信息，可以包含用户id，角色等信息。信息类型可以自定义比如 new Claim("xx","xx")
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, uid),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,                				  new Claim(JwtRegisteredClaimNames.Aud,"user1"),
            };

            // 可以将一个用户的多个角色全部赋予；各角色之间用，分割
            // 作者：DX 提供技术支持；
            claims.AddRange(role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

            // 秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			// 整理成一个token对象
            var jwt = new JwtSecurityToken(
                issuer: "zyz",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            // 处理成token字符串
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }
```

### 4、授权与授权策略

### 5、鉴权

