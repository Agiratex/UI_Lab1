// pch.h: это предварительно скомпилированный заголовочный файл.
// Перечисленные ниже файлы компилируются только один раз, что ускоряет последующие сборки.
// Это также влияет на работу IntelliSense, включая многие функции просмотра и завершения кода.
// Однако изменение любого из приведенных здесь файлов между операциями сборки приведет к повторной компиляции всех(!) этих файлов.
// Не добавляйте сюда файлы, которые планируете часто изменять, так как в этом случае выигрыша в производительности не будет.

#ifndef PCH_H
#define PCH_H

// Добавьте сюда заголовочные файлы для предварительной компиляции
#include <chrono>
#include "framework.h"
#include "mkl_vml.h"
#include <iostream>

// errors
constexpr int WRONG_FUNCTION = 1;


// function using mkl
extern "C" _declspec(dllexport) int calculate_d_function(int length, double vector[], int function_code,
    double res_HA[], double res_EP[], double time[]);

    extern "C" _declspec(dllexport) int calculate_f_function(int length, float vector[], int function_code,
        float res_HA[], float res_EP[], double time[]);

enum VMfCpp
{
    vmdLn_code,
    vmdLGamma_code,
    vmsLn_code,
    vmsLGamma_code,
};

// Timer for time counting
class Timer
{
private:
    using clock_t = std::chrono::high_resolution_clock;
    using second_t = std::chrono::duration<double, std::ratio<1> >;
    std::chrono::time_point<clock_t> time;
public:
    // constructor
    Timer() : time(clock_t::now()) {}

    // methods
    void reset();
    double elapsed() const;
};

#endif //PCH_H
