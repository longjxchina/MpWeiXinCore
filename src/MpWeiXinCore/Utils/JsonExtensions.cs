using Newtonsoft.Json;

namespace MpWeiXinCore.Utils
{
    internal static class JsonExtensions
    {
        #region 函数

        /// <summary>
        /// 序列化一个对象
        /// </summary>
        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T">object</typeparam>
        /// <param name="value">json字符串</param>
        /// <returns>返回实例对象</returns>
        public static TObject ToObject<TObject>(this string value)
        {
            TObject result = default(TObject);
            if (typeof(TObject) == typeof(string))
            {
                object tempSwap = (object)value;
                result = (TObject)tempSwap;
            }
            else if (!string.IsNullOrEmpty(value))
            {
                result = JsonConvert.DeserializeObject<TObject>(value);
            }

            return result;
        }

        #endregion
    }
}
