// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"

// Timer methods implementation
void Timer::reset()
{
	time = clock_t::now();
}

double Timer::elapsed() const
{
	return std::chrono::duration_cast<second_t>(clock_t::now() - time).count();
}

//Call MKL functions

//Call for double type
int
calculate_d_function(int length, double vector[], int function_code,
	double res_HA[], double res_EP[], double time[])
{
	if (function_code == vmdLn_code)
	{
		Timer timer;
		vmdLn(length, vector, res_HA, VML_HA);
		time[0] = timer.elapsed();

		timer.reset();
		vmdLn(length, vector, res_EP, VML_EP);
		time[1] = timer.elapsed();

		return 0;
	}
	else if (function_code == vmdLGamma_code) 
	{
		Timer timer;
		vmdLGamma(length, vector, res_HA, VML_HA);
		time[0] = timer.elapsed();

		timer.reset();
		vmdLGamma(length, vector, res_EP, VML_EP);
		time[1] = timer.elapsed();

		return 0;
	}
	else 
	{
		return WRONG_FUNCTION;
	}
}


//Call for single type
int
calculate_f_function(int length, float vector[], int function_code,
	float res_HA[], float res_EP[], double time[])
{
	if (function_code == vmsLn_code)
	{
		Timer timer;
		vmsLn(length, vector, res_HA, VML_HA);
		time[0] = timer.elapsed();
		timer.reset();
		vmsLn(length, vector, res_EP, VML_EP);
		time[1] = timer.elapsed();

		return 0;
	}
	else if (function_code == vmsLGamma_code)
	{
		Timer timer;
		vmsLGamma(length, vector, res_HA, VML_HA);
		time[0] = timer.elapsed();

		timer.reset();
		vmsLGamma(length, vector, res_EP, VML_EP);
		time[1] = timer.elapsed();

		return 0;
	}
	else
	{
		return WRONG_FUNCTION;
	}
}