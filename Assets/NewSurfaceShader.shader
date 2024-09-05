Shader "Custom/SingleFaceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert
        
        struct Input
        {
            float2 uv_MainTex;
        };
        
        sampler2D _MainTex;
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            float2 uv = IN.uv_MainTex;
            
            // Masquer les autres faces
            if (uv.x < 0.5 || uv.y < 0.5) // Modifier ces conditions selon votre besoin
            {
                discard;
            }
            
            // Appliquer la texture uniquement sur la face choisie
            o.Albedo = tex2D (_MainTex, uv);
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
