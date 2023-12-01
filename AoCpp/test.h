class test {
	int a;
public:
	test(int x);
	int adding(int y);
};


extern "C" __declspec(dllexport) void* Create(int x) {
	return (void*) new test(x);
}
extern "C" __declspec(dllexport) int testAdd(test * a, int y) {
	return a->adding(y);
}