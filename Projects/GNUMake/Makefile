SRCDIR = ../../Source
GFXDIR = ../../Graphics
SRCS = $(GFXDIR)/RenderAPI.cpp \
$(GFXDIR)/RenderAPI_OpenGL2.cpp \
$(GFXDIR)/RenderAPI_OpenGLCoreES.cpp
OBJS = ${SRCS:.cpp=.o}
#SUPPORT_OPENGL_LEGACY Legacy OpenGL, removed
UNITY_DEFINES = -DSUPPORT_OPENGL_LEGACY=0 -DSUPPORT_OPENGL_UNIFIED=1 -DUNITY_LINUX=1
GLEW_CFLAGS = $(shell pkg-config --cflags glew)
GLEW_LIBS = $(shell pkg-config --libs glew)
CXXFLAGS = $(UNITY_DEFINES) -O2 -fPIC $(GLEW_CFLAGS)
LDFLAGS = -shared -rdynamic
LIBS = $(GLEW_LIBS)
PLUGIN_SHARED = unity_webrtc.so
CXX ?= g++

.cpp.o:
	$(CXX) $(CXXFLAGS) -c -o $@ $<

all: shared

clean:
	rm -f $(OBJS) $(PLUGIN_SHARED)

shared: $(OBJS)
	$(CXX) $(LDFLAGS) -o $(PLUGIN_SHARED) $(OBJS) $(LIBS)
