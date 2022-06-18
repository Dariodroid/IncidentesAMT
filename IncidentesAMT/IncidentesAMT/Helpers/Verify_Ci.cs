using System;
using System.Collections.Generic;
using System.Text;

namespace IncidentesAMT.Helpers
{
    public static class Verify_Ci
    {
        public static bool VerificaIdentificacion(string identificacion)
        {
            try
            {
                bool estado = false;
                char[] valced = new char[13];
                int provincia;
                if (identificacion.Length >= 10)
                {
                    valced = identificacion.Trim().ToCharArray();
                    provincia = int.Parse((valced[0].ToString() + valced[1].ToString()));
                    if (provincia > 0 && provincia < 25)
                    {
                        if (int.Parse(valced[2].ToString()) < 6)
                        {
                            estado = VerificaCedula(valced);
                        }
                        else if (int.Parse(valced[2].ToString()) == 6)
                        {
                            estado = VerificaSectorPublico(valced);
                        }
                        else if (int.Parse(valced[2].ToString()) == 9)
                        {

                            estado = VerificaPersonaJuridica(valced);
                        }
                    }
                }
                return estado;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        //el método VerificaIdentificacion recibe como parametro identificacion que sera
        //nuestra cedula/RUC a verificar, tengo una variable estado que es lo que voy a devolver,
        //si la identificacion es mayor o igual a 10 digitos voy al siguiente paso.
        //Voy a tomar la variable de ingreso(identificacion) y la voy a pasar a un array con el fin
        //de recorrer este array en los siguientes metodos, tambien me va a servir para los controles
        //previos; en este caso tomo los primeros dos caracteres que se identifican como la provincia
        //y los paso a una variable int, verifico que este número se encuentre entre 1 y 24
        //(que son las provincias que existen al momento en Ecuador).
        //Ahora continuo validando el tercer digito, este identifica al tipo de
        //persona(Natural, Juridico o Sector Publico), para una persona natural el numero debe estar
        //comprendido entre 0 y 6, para el sector publico el tercer dígito siempre será 6 y para una
        //persona Jurídica es 9.
        public static bool VerificaCedula(char[] validarCedula)
        {
            try
            {
                int aux = 0, par = 0, impar = 0, verifi;
                for (int i = 0; i < 9; i += 2)
                {
                    aux = 2 * int.Parse(validarCedula[i].ToString());
                    if (aux > 9)
                        aux -= 9;
                    par += aux;
                }
                for (int i = 1; i < 9; i += 2)
                {
                    impar += int.Parse(validarCedula[i].ToString());
                }

                aux = par + impar;
                if (aux % 10 != 0)
                {
                    verifi = 10 - (aux % 10);
                }
                else
                    verifi = 0;
                if (verifi == int.Parse(validarCedula[9].ToString()))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //El coeficiente es el valor por el que se debe multiplicar cada uno de los digitos,
        //en el código hay dos for los cuales recorren el parametro de ingreso de 2 en 2; en el
        //caso de los pares guardo el resultado en una variable auxiliar para que, si el resultado
        //de la multiplicación es mayor a 9 resto ese número y guardo el valor de la variable aux en
        //la variable par; en el caso de los impares solo sumo los valores en una variable.
        //Al final sumo los valores de la variable par e impar y realizo el mod para recuperar el
        //digito verificador.
        public static bool VerificaPersonaJuridica(char[] validarCedula)
        {
            try
            {
                int aux = 0, prod, veri;
                veri = int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
                if (veri > 0)
                {
                    int[] coeficiente = new int[9] { 4, 3, 2, 7, 6, 5, 4, 3, 2 };
                    for (int i = 0; i < 9; i++)
                    {
                        prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                        aux += prod;
                    }
                    if (aux % 11 == 0)
                    {
                        veri = 0;
                    }
                    else if (aux % 11 == 1)
                    {
                        return false;
                    }
                    else
                    {
                        aux = aux % 11;
                        veri = 11 - aux;
                    }

                    if (veri == int.Parse(validarCedula[9].ToString()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        //En el caso de las personas jurídicas el dígito verificador es el décimo dígito,
        //al igual que la cédula en la imagen pueden ver los coeficientes por los que se debe
        //realizar la multiplicación de los dígitos {4,3,2,7,6,5,4,3,2}.
        public static bool VerificaSectorPublico(char[] validarCedula)
        {
            try
            {
                int aux = 0, prod, veri;
                veri = int.Parse(validarCedula[9].ToString()) + int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
                if (veri > 0)
                {
                    int[] coeficiente = new int[8] { 3, 2, 7, 6, 5, 4, 3, 2 };

                    for (int i = 0; i < 8; i++)
                    {
                        prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                        aux += prod;
                    }

                    if (aux % 11 == 0)
                    {
                        veri = 0;
                    }
                    else if (aux % 11 == 1)
                    {
                        return false;
                    }
                    else
                    {
                        aux = aux % 11;
                        veri = 11 - aux;
                    }

                    if (veri == int.Parse(validarCedula[8].ToString()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
