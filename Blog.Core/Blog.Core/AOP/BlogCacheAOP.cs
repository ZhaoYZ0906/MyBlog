using Blog.Core.Common.Attributes;
using Blog.Core.Interface.IRedis;
using Castle.DynamicProxy;

namespace Blog.Core.Api.AOP
{
    /// <summary>
    /// 面向切面的缓存使用
    /// </summary>
    public class BlogCacheAOP : IInterceptor
    {
        //通过注入的方式，把缓存操作接口通过构造函数注入
        private ICaching _cache;
        private IRedisBasketRepository _redisCache;

        public BlogCacheAOP(ICaching cache, IRedisBasketRepository redisCache)
        {
            _cache = cache;
            _redisCache= redisCache;
        }

        //Intercept方法是拦截的关键所在，也是IInterceptor接口中的唯一定义
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            var qCachingAttribute = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) as CachingAttribute;
            //只有那些指定的才可以被缓存，需要验证
            if (qCachingAttribute != null)
            {
                //获取自定义缓存键
                var cacheKey = CustomCacheKey(invocation);

                //根据key获取相应的缓存值
                var cacheValue = _cache.Get(cacheKey);
                if (cacheValue != null)
                {
                    //将当前获取到的缓存值，赋值给当前执行方法
                    invocation.ReturnValue = cacheValue;
                    return;
                }
                //去执行当前的方法
                invocation.Proceed();
                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    _cache.Set(cacheKey, invocation.ReturnValue, qCachingAttribute.AbsoluteExpiration);
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }
        }


        //public async void Intercept(IInvocation invocation)
        //{
        //    var method = invocation.MethodInvocationTarget ?? invocation.Method;
        //    //对当前方法的特性验证
        //    var qCachingAttribute = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) as CachingAttribute;
        //    //只有那些指定的才可以被缓存，需要验证
        //    if (qCachingAttribute != null)
        //    {
        //        //获取自定义缓存键
        //        var cacheKey = CustomCacheKey(invocation);

        //        //根据key获取相应的缓存值
        //        var cacheValue = await _redisCache.GetValue(cacheKey);
        //        if (cacheValue != null)
        //        {
        //            //将当前获取到的缓存值，赋值给当前执行方法

        //            // 如果执行的方法返回值为空则直接返回
        //            var type = invocation.Method.ReturnType;
        //            var resultTypes = type.GenericTypeArguments;
        //            if (type.FullName == "System.Void")
        //            {
        //                return;
        //            }


        //            object response;
        //            if (type != null && typeof(Task).IsAssignableFrom(type))
        //            {
        //                //核心2：返回异步对象Task<T>
        //                if (resultTypes.Count() > 0)
        //                {
        //                    var resultType = resultTypes.FirstOrDefault();
        //                    // 核心3，直接序列化成 dynamic 类型，之前我一直纠结特定的实体
        //                    dynamic temp = Newtonsoft.Json.JsonConvert.DeserializeObject(cacheValue, resultType);
        //                    response = Task.FromResult(temp);

        //                }
        //                else
        //                {
        //                    //Task 无返回方法 指定时间内不允许重新运行
        //                    response = Task.Yield();
        //                }
        //            }
        //            else
        //            {
        //                // 核心4，要进行 ChangeType
        //                response = System.Convert.ChangeType(_redisCache.Get<object>(cacheKey), type);
        //            }

        //            invocation.ReturnValue = response;
        //            return;
        //        }
        //        //去执行当前的方法
        //        invocation.Proceed();
        //        //存入缓存
        //        if (!string.IsNullOrWhiteSpace(cacheKey))
        //        {
        //            _cache.Set(cacheKey, invocation.ReturnValue, qCachingAttribute.AbsoluteExpiration);
        //        }


        //    }
        //    else
        //    {
        //        invocation.Proceed();//直接执行被拦截方法
        //    }
        //}


        //自定义缓存键
        private string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();//获取参数列表，我最多需要三个即可

            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key += $"{param}:";
            }

            return key.TrimEnd(':');
        }
        //object 转 string
        private string GetArgumentValue(object arg)
        {
            // PS：这里仅仅是很简单的数据类型，如果参数是表达式/类等，比较复杂的，请看我的在线代码吧，封装的比较多，当然也可以自己封装。
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            return "";
        }
    }
}
