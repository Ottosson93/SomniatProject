Shader "Custom/NightmareScale"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Center ("Circle Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Circle Radius", Range(0, 0.5)) = 0.2
        

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Center;
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = _Center.xy;
                float2 distance = i.uv - center;

              //  distance.y *= 0.5;

                float distanceFromCenter = length(distance);

                // Add horror pulsating effect
                float pulsateAmount = sin(_Time.y * 6) * 0.003;

                // Adjust the radius based on pulsating effect
                float adjustedRadius = _Radius + pulsateAmount;

                fixed4 col = tex2D(_MainTex, i.uv);

                // Check if the fragment is outside the circle
                if (distanceFromCenter > adjustedRadius)
                {
                    // Apply distortion

                    float distortion = sin(distanceFromCenter * 40) * 0.02; // Adjust the distortion intensity
                    float2 distortedUV = i.uv + distance * distortion;

                    // Sample the texture with distorted UV
                    col = tex2D(_MainTex, distortedUV);

                    // Convert to grayscale
                    /*float intensity = col.r * 0.3 + col.g * 0.59 + col.b * 0.11;*/

                    // Convert to grayscale
                    float intensity = (col.r + col.g + col.b) / 1.6;

                    // Apply the grayscale color
                    // Apply a bluish tint
                    col = fixed4(intensity * 0.2, intensity * 0.3, intensity * 0.3, 0.5);;

                    return col;
                }
                else
                {
                    // Return the original scene color inside the circle
                    return col;
                }
            }
            ENDCG
        }
    }
}
