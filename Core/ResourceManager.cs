using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UN2WT {
    public class ResourceManager {

        public Dictionary<Texture2D, string> Textures;
        public Dictionary<Material, string> Materials;

        public ResourceManager() {
            Textures = new Dictionary<Texture2D, string>();
            Materials = new Dictionary<Material, string>();
        }

        public string ProcessTexture(Texture2D texture) {
            if(texture == null) return "";
            if(!Textures.ContainsKey(texture)) {
                string path = AssetDatabase.GetAssetPath(texture).Remove(0, 7).ToLower();
                if(path == null || path == "") return "";
                Textures.Add(texture, path);
            }
            return Textures[texture];
        }
        
        public string ProcessMaterial(Material material) {
            if(material == null) return "";
            if(!Materials.ContainsKey(material)) {
                string path = AssetDatabase.GetAssetPath(material).Remove(0, 7).ToLower();
                if(path == null || path == "") return "";
                Materials.Add(material, path);
            }
            return Materials[material];
        }

    }
}