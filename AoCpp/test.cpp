#include "pch.h"
#include "test.h"
test::test(int a) {
	this->a = a;
}
int test::adding(int y)
{
	return this->a + y;
}