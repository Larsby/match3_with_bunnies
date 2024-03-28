// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/NewSurfaceShader" {
	    Properties
    {
        // we have removed support for texture tiling/offset,
        // so make them not be displayed in material inspector
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
            };

            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                // transform position to clip space
                // (multiply with model*view*projection matrix)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // just pass the texture coordinate

              //  o.vertex.x += _SinTime*0.1;
             //   o.vertex.y += _CosTime*0.1;
                o.uv = v.uv;
                  //   o.uv.x += _SinTime*100.1;
       //     o.uv.y += _SinTime*0.1;
                return o;
            }
            
            // texture we will sample
            sampler2D _MainTex;

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                // sample texture and return it
                float v = 0;
                //https://answers.unity.com/questions/767107/how-to-use-shader-sintime.html
                //https://www.bidouille.org/prog/plasma
                v+=i.uv.x+_SinTime.w;
                v+=i.uv.y+_SinTime.z;
                v+=sin(i.uv.x+i.uv.y+_SinTime.z)/2.0;
                v*=0.5;
                v+=0.5;
                float2 myuv = v-i.uv;
              //  myuv = clamp(myuv,float2(0.4,1.0),float2(0.4,1.0));
                myuv*=0.5;
            //    i.uv.x+=(_SinTime.w*0.5);
          //      i.uv.y+=(_SinTime.w*0.5);
                /*
                 float v = 0.0;
         vec2 c = v_coords * u_k - u_k/2.0;
    v += sin((c.x+u_time));
    v += sin((c.y+u_time)/2.0);
    v += sin((c.x+c.y+u_time)/2.0);
    c += u_k/2.0 * vec2(sin(u_time/3.0), cos(u_time/2.0));
    v += sin(sqrt(c.x*c.x+c.y*c.y+1.0)+u_time);
    v = v/2.0;
    vec3 col = vec3(1, sin(PI*v), cos(PI*v));
    gl_FragColor = vec4(col*.5 + .5, 1);

    */


   // v = clamp(v,-0,1.0);
                fixed4 col = tex2D(_MainTex,myuv );
                col+= tex2D(_MainTex,i.uv )*0.2;
              //  col.r+=v;
               // col.g-=v;
                return col;
            }
            ENDCG
        }
    }
}