/*
 * Created by SharpDevelop.
 * User: skhal_000
 * Date: 10/13/2015
 * Time: 5:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
namespace Gondathon
{
	class XmlApi
	{
		XmlDocument doc = null;
		public void LoadXml(string xmlFile){
			doc = new XmlDocument();
			doc.Load(xmlFile);
		}
		
		public string GetIcon(string title){
			var elemList = doc.GetElementsByTagName("child");
			string icon=null;
			foreach ( XmlElement elem in elemList){
				if (title.Equals(elem.GetAttribute("id").ToString())){
					icon = elem.GetElementsByTagName("src")[0].InnerText;
					break;
				 }
	
			}

			return icon;
		}
		public string GetEvent(string title){
			var elemList = doc.GetElementsByTagName("child");
			string eventString=null;
			foreach ( XmlElement elem in elemList){
				if (title.Equals(elem.GetAttribute("id").ToString())){
					eventString = elem.GetElementsByTagName("number")[0].InnerText;
					break;
				 }
	
			}
			return eventString;
		}
		
		public List<string> GetChildren(string title){
			var elemList = doc.GetElementsByTagName("child");
			List<string> children=null;
			foreach ( XmlElement elem in elemList){
				if (title.Equals(elem.GetAttribute("id").ToString())){
					var childrenElements = elem.GetElementsByTagName("child");
					children = new List<string>();
					foreach ( XmlElement e in childrenElements){
						children.Add(e.GetAttribute("id").ToString());
					}
					break;
				 }
	
			}
			return children;
		}
		
		public string GetRoot(){
			XmlElement rootElement = (XmlElement)doc.GetElementsByTagName("root")[0];
			string root=rootElement.GetAttribute("id").ToString();
			return root;	
			
		}
		
		public string GetFather(string title){
			var elemList = doc.GetElementsByTagName("child");
			string father = null;
			foreach ( XmlElement elem in elemList){
				if (title.Equals(elem.GetAttribute("id").ToString())){
					XmlElement fatherElement = (XmlElement)elem.SelectSingleNode("..");
					father = fatherElement.GetAttribute("id").ToString();
					break;
				}
			}
			return father;
		}
		
		public void SendEvent(string title){
			Console.Write(GetEvent(title));
		}
	}
}