using System;

namespace Assets.Scripts {

    class RandomExponential : System.Random {

        /// <summary>
        /// Сохраняем в приватном поле параметр распределения и расчетный коефициент, чтоб не вычислять его при каждой генерации
        /// </summary>
        private double L;

        /// <summary>
        /// параметр λ 
        /// </summary>
        private double l;

        /// <summary>
        /// Инициализирует генератор случайных чисел с экпоненциальным распределением и параметром λ = 1
        /// </summary>
        public RandomExponential() : this(1) { }

        /// <summary>
        /// Инициализирует генератор случайных чисел с экпоненциальным распределением и параметром λ 
        /// </summary>
        /// <param name="l">λ - параметр </param>
        public RandomExponential(double l) : base() {
            this.l = l;
            L = Math.Pow(Math.E, l) - 1;
        }
        
        /// <summary>
        /// Этот ГСЧ основан на нормальном распеределении.
        /// Для преобразования нормального распределения берем обратную экспонениальной функции - натуральный логарифм
        /// </summary>
        /// <returns></returns>
        protected override double Sample() {
            return 1 - Math.Log(base.Sample() * L + 1) / l;
        }

        /// <summary>
        /// Этот метод в базовом классе не зависит от метода Sample, потому его следует так же переопределить
        /// </summary>
        /// <returns></returns>
        public override int Next() {
            return (int)(Sample() * int.MaxValue);
        }
    }
}
