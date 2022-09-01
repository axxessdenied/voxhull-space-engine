/*
/ Author    : Nick Slusarczyk
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
	using UnityEngine;
	using UnityEditor;

	public class KeywordReplace : UnityEditor.AssetModificationProcessor {
 
		/*public static void OnWillCreateAsset ( string path ) {
			path = path.Replace( ".meta", "" );
			var index = path.LastIndexOf( "." );
			var file = path.Substring( index );
			if ( file != ".cs" && file != ".js" && file != ".boo" ) return;
			index = Application.dataPath.LastIndexOf( "Assets" );
			path = Application.dataPath.Substring( 0, index ) + path;
			file = System.IO.File.ReadAllText( path );
 
			file = file.Replace( "#CREATIONDATE#", System.DateTime.Now + "" );
			file = file.Replace( "#PROJECTNAME#", PlayerSettings.productName );
			file = file.Replace( "#PROJECTDEVELOPERS#", PlayerSettings.companyName );
			file = file.Replace( "#COPYPASTE#", GUIUtility.systemCopyBuffer );

			System.IO.File.WriteAllText( path, file );
			AssetDatabase.Refresh();
			
		}
		*/
	}
}