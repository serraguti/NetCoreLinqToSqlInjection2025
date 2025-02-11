using NetCoreLinqToSqlInjection.Models;

namespace NetCoreLinqToSqlInjection.Repositories
{
    public interface IRepositoryDoctores
    {
        List<Doctor> GetDoctores();

        void InsertDoctor
            (int idDoctor, string apellido, string especialidad
            , int salario, int idHospital);

        void DeleteDoctor(int idDoctor);
        List<Doctor> GetDoctoresEspecialidad(string especialidad);
        void UpdateDoctor(int idDoctor, string apellido
            , string especialidad, int salario, int idHospital);
        Doctor FindDoctor(int idDoctor);
    }
}
