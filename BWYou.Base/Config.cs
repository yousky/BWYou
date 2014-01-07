using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace BWYou.Base
{
    /// <summary>
    /// 환경 값 처리 클래스
    /// </summary>
    public class Config : ClassBase
    {
        /// <summary>
        /// XML 환경 파일 내부의 appSettings 값을 읽어서 Dictionary 형식으로 돌려주기
        /// </summary>
        /// <param name="filename">읽을 XML 환경 파일</param>
        /// <param name="dicConfigKeyValue">appSettings의 Dictionary 형식 값</param>
        /// <returns></returns>
        public bool ReadConfigAppSettings(string filename, out Dictionary<string, string> dicConfigKeyValue)
        {
            dicConfigKeyValue = new Dictionary<string, string>();
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(filename);
                XmlElement root = xmldoc.DocumentElement;

                // 노드 요소들
                XmlNodeList nodes = root.ChildNodes;

                // 노드 요소의 값을 읽어 옵니다.
                foreach (XmlElement node in nodes)
                {
                    if (node.Name == "appSettings")
                    {
                        foreach (XmlElement childNode in node)
                        {
                            string key = childNode.Attributes["key"].Value;
                            string value = childNode.Attributes["value"].Value;
                            if (dicConfigKeyValue.ContainsKey(key) == false)
                            {
                                dicConfigKeyValue.Add(key, value);
                            }
                            else
                            {
                                SayMessage(this, "XML config 동일 환경 값 존재. 무시 됨", MessagePriority.Warn);
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                SayMessage(this, "XML config 읽기 실패 : " + ex.Message, MessagePriority.Error);
                return false;
            }
        }
    }
}
