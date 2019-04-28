using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCView.Core.Util;
using XCView.Redis;
using XCView.Core.ThirdPartyReference;

namespace ModelManager.RedisAccess
{
    public class RedisHelper
    {

        //前缀
        public static readonly string PrefixKey = "qskj.model.redis";
        //默认过期时间
        public static readonly TimeSpan RedisExpires = new TimeSpan(1, 0, 0, 0);

        public static readonly string Zero = "0";

        /// <summary>
        /// 从redis 获取数据  并解析为 指定类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheSetter"></param>
        /// <returns></returns>
        public static T GetProtobuf<T>(string key, Func<T> cacheSetter) where T : class
        {
            //Redis有数据，则直接从Redis获取
            T value = null;
            try
            {
                value = RedisClient.GetProtobuf<T>(key);
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(string.Format("读取Reids异常（{0}）", key), ex);
            }

            //Redis无数据,则直接从CacheSetter获取
            value = cacheSetter();
            if (value != null)
            {
                try
                {
                    RedisClient.SetProtobuf(key, value);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteException(string.Format("写入Reids异常（{0}）", key), ex);
                }
            }
            return value;
        }



        /// <summary>
        /// 根据键 从redis 获取数据 并且转化为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashID"></param>
        /// <param name="cacheSetter"></param>
        /// <returns></returns>
        public static List<T> GetHash<T>(string hashID, Func<List<T>> cacheSetter) where T : class
        {
            //Redis有数据，则直接从Redis获取
            List<T> list = null;
            try
            {

                list = RedisClient.Using(x => x.GetAllEntriesFromHash(hashID))
                    .Select(x => x.Value.DeserializeJson<T>()).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(string.Format("读取Reids异常（{0}）", hashID), ex);
            }

            if (list == null)
            {
                //Redis无数据,则直接从CacheSetter获取
                list = cacheSetter();
                //if (list != null)
                //{
                //    try
                //    {
                //        RedisClient.SetProtobuf(hashID, list);
                //    }
                //    catch (Exception ex)
                //    {
                //        LogHelper.WriteException(string.Format("写入Reids异常（{0}）", hashID), ex);
                //    }
                //}
            }
            return list;
        }


        /// <summary>
        /// 将数据 添加到 redis中
        /// </summary>
        /// <param name="hashID"></param>
        /// <param name="dic"></param>
        /// <param name="cacheSetter"></param>
        /// <returns></returns>
        public static bool SetHash(string hashID, Dictionary<string, string> dic, Func<bool> cacheSetter)
        {
            var suss = cacheSetter();
            var result = false;
            if (suss)
            {
                try
                {
                    RedisClient.Using(x => x.SetRangeInHash(hashID, dic));
                    result = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteException(string.Format("写入Reids异常（{0}::{1}）", hashID), ex);
                }
            }
            return result;
        }

        /// <summary>
        /// 设置redis hash 表
        /// </summary>
        /// <param name="hashID"></param>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="cacheSetter"></param>
        /// <returns></returns>
        public static bool SetHash(string hashID, string key, string obj, Func<bool> cacheSetter)
        {
            var suss = cacheSetter();
            var result = false;
            if (suss)
            {
                try
                {
                    result = RedisClient.Using(x => x.SetEntryInHash(hashID, key, obj));
                }
                catch (Exception ex)
                {
                    LogHelper.WriteException(string.Format("写入Reids异常（{0}::{1}）", hashID, key), ex);
                }
            }
            return result;
        }



        #region 生成键值对

        /// <summary>
        /// 微信公众号 信息键
        /// </summary>
        /// <returns></returns>
        public static string RenderWeixinInfoKey()
        {
            return RedisClient.RenderRedisKey(RedisHelper.PrefixKey, "weixin_info");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RenderRandomsKey()
        {
            return RedisClient.RenderRedisKey(RedisHelper.PrefixKey, "RandomKeys");
        }


        #endregion

    }
}
