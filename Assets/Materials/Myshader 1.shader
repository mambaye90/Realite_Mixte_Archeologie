Shader "Custom/FragmentMaskShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        CGPROGRAM
        #pragma surface surf Lambert
        
        struct Input
        {
            float3 worldNormal;
        };
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Masquer la partie fragment
            if (IN.worldNormal.z < 0)
                discard;
            
            o.Albedo = _Color.rgb;
            o.Alpha = _Color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
