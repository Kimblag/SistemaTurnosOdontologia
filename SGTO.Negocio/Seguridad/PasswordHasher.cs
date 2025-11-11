using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SGTO.Negocio.Seguridad
{
    public static class PasswordHasher
    {

        // esta clase es para hashear las pass de los usuarios.
        // utiliza el namespace cryptography para poder usar lo necesario al momento de hashear
        // fuente: https://stackoverflow.com/questions/4181198/how-to-hash-a-password#10402129

        private const int TAMANIO_SALT = 16; // decimos de cuánto sera la secuencia aleatoria que le agregaremos a la pass, permite no tener dos pass iguales con el mismo hash
        private const int TAMANIO_HASH = 32; // este es el tamaño final que tendrá el hash, luego de las iteraciones, serán 256 bits en est ecaso
        private const int ITERACIONES = 100000; // con esto se determina cuantas veces vamos a aplicar el hash, puede ser cambiada

        // método para generar hash seguro PBKDF2 con SHA1
        public static string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña no puede estar vacía.");

            // creamos el salt aleatorio
            byte[] salt = new byte[TAMANIO_SALT];
            new RNGCryptoServiceProvider().GetBytes(salt);

            // derivamos la clave a PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERACIONES);
            byte[] hash = pbkdf2.GetBytes(TAMANIO_HASH);

            byte[] hashBytes = new byte[TAMANIO_SALT + TAMANIO_HASH];
            Array.Copy(salt, 0, hashBytes, 0, TAMANIO_SALT);
            Array.Copy(hash, 0, hashBytes, TAMANIO_SALT, TAMANIO_HASH);

            return Convert.ToBase64String(hashBytes);
        }


        public static bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(storedHash))
                return false;

            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // para verificar se tiene que extraer el salt (primeros 16 bytes)
            byte[] salt = new byte[TAMANIO_SALT];
            Array.Copy(hashBytes, 0, salt, 0, TAMANIO_SALT);

            // se deriva la clave con la misma salt que fue hasheada
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERACIONES);
            byte[] hash = pbkdf2.GetBytes(TAMANIO_HASH);

            // manualmente comparar byte a byte
            for (int i = 0; i < TAMANIO_HASH; i++)
            {
                if (hashBytes[i + TAMANIO_SALT] != hash[i])
                    return false;
            }

            return true;
        }


    }
}
