#ifndef __COMMOM__
#define __COMMOM__

#include "UnityCG.cginc"

struct node
{
	float scale;
	float2 uv ;
};


node getScale(float t) 
{
	node n;
	n.uv = float2(t, 1-t);
	n.scale = clamp(0, 1, t*0.2);
	return n;
}


#endif