Shader "Unlit/FireTrailShader"
{
    Properties
    {
        _MainTex("Trail Texture", 2D) = "white" {}
    }

        SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            float2 uv = IN.uv_MainTex;
            float centerDist = length(uv - 0.5);

            // White in the center, transitioning to orange at the edges
            float3 color = lerp(float3(1, 1, 1), float3(1, 0.5, 0), centerDist);

            o.Albedo = color;
        }
        ENDCG
    }

        FallBack "Diffuse"

}
