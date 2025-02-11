using NetCoreLinqToSqlInjection.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Security.Cryptography.Xml;

#region PROCEDIMIENTOS ALMACENADOS
/*
--LENGUAJE PL/SQL
create or replace procedure sp_delete_doctor
(p_iddoctor DOCTOR.DOCTOR_NO%TYPE)
as
begin
  delete from DOCTOR where DOCTOR_NO=p_iddoctor;
  commit;
end; 
create or replace procedure sp_update_doctor
(p_iddoctor DOCTOR.DOCTOR_NO%TYPE
, p_apellido DOCTOR.APELLIDO%TYPE
, p_especialidad DOCTOR.ESPECIALIDAD%TYPE
, p_salario DOCTOR.SALARIO%TYPE
, p_idhospital DOCTOR.HOSPITAL_COD%TYPE)
as
begin
  update DOCTOR set APELLIDO=p_apellido, ESPECIALIDAD=p_especialidad
  , SALARIO=p_salario, HOSPITAL_COD=p_idhospital
  where DOCTOR_NO=p_iddoctor;
  commit;
end;
 */
#endregion

namespace NetCoreLinqToSqlInjection.Repositories
{
    public class RepositoryDoctoresOracle : IRepositoryDoctores
    {
        private DataTable tablaDoctores;
        private OracleConnection cn;
        private OracleCommand com;

        public RepositoryDoctoresOracle()
        {
            string connectionString = 
                @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.tablaDoctores = new DataTable();
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            OracleDataAdapter ad =
                new OracleDataAdapter("select * from DOCTOR", connectionString);
            ad.Fill(this.tablaDoctores);
        }

        public void DeleteDoctor(int idDoctor)
        {
            string sql = "sp_delete_doctor";
            OracleParameter pamId =
                new OracleParameter(":p_iddoctor", idDoctor);
            this.com.Parameters.Add(pamId);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public List<Doctor> GetDoctores()
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor();
                doc.IdDoctor = row.Field<int>("DOCTOR_NO");
                doc.Apellido = row.Field<string>("APELLIDO");
                doc.Especialidad = row.Field<string>("ESPECIALIDAD");
                doc.Salario = row.Field<int>("SALARIO");
                doc.IdHospital = row.Field<int>("HOSPITAL_COD");
                doctores.Add(doc);
            }
            return doctores;
        }

        public void InsertDoctor(int idDoctor, string apellido, string especialidad, int salario, int idHospital)
        {
            string sql = "insert into DOCTOR values (:idhospital, :iddoctor "
                + ", :apellido, :especialidad, :salario)";
            //ORACLE TIENE EN CUENTA EL ORDEN DE LOS PARAMETROS
            OracleParameter pamHospital = new OracleParameter(":idhospital", idHospital);
            this.com.Parameters.Add(pamHospital);
            OracleParameter pamIdDoctor = 
                new OracleParameter(":iddoctor", idDoctor);
            this.com.Parameters.Add(pamIdDoctor);
            OracleParameter pamApellido = 
                new OracleParameter(":apellido", apellido);
            this.com.Parameters.Add(pamApellido);
            OracleParameter pamEspecialidad =
                new OracleParameter(":especialidad", especialidad);
            this.com.Parameters.Add(pamEspecialidad);
            OracleParameter pamSalario = new OracleParameter(":salario", salario);
            this.com.Parameters.Add(pamSalario);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void UpdateDoctor(int idDoctor, string apellido
            , string especialidad, int salario, int idHospital)
        {
            string sql = "sp_update_doctor";
            OracleParameter pamId =
                new OracleParameter(":p_iddoctor", idDoctor);
            this.com.Parameters.Add(pamId);
            OracleParameter pamApe =
                new OracleParameter(":p_apellido", apellido);
            this.com.Parameters.Add(pamApe);
            OracleParameter pamEspe =
                new OracleParameter(":p_especialidad", especialidad);
            this.com.Parameters.Add(pamEspe);
            OracleParameter pamSal = new OracleParameter(":p_salario", salario);
            this.com.Parameters.Add(pamSal);
            OracleParameter pamHosp =
                new OracleParameter(":p_idhospital", idHospital);
            this.com.Parameters.Add(pamHosp);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public Doctor FindDoctor(int idDoctor)
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           where datos.Field<int>("DOCTOR_NO") == idDoctor
                           select datos;
            var row = consulta.First();
            Doctor doc = new Doctor();
            doc.IdDoctor = row.Field<int>("DOCTOR_NO");
            doc.Apellido = row.Field<string>("APELLIDO");
            doc.Especialidad = row.Field<string>("ESPECIALIDAD");
            doc.Salario = row.Field<int>("SALARIO");
            doc.IdHospital = row.Field<int>("HOSPITAL_COD");
            return doc;
        }

        public List<Doctor> GetDoctoresEspecialidad(string especialidad)
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
where (datos.Field<string>("ESPECIALIDAD")).ToUpper()
                           == especialidad.ToUpper()
                           select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor();
                doc.IdDoctor = row.Field<int>("DOCTOR_NO");
                doc.Apellido = row.Field<string>("APELLIDO");
                doc.Especialidad = row.Field<string>("ESPECIALIDAD");
                doc.Salario = row.Field<int>("SALARIO");
                doc.IdHospital = row.Field<int>("HOSPITAL_COD");
                doctores.Add(doc);
            }
            return doctores;
        }


    }
}
