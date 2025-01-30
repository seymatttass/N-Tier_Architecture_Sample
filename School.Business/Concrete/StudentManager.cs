using AutoMapper; // Nesneleri dönüştürmek için kullanılan AutoMapper kütüphanesi
using Core.Business.DTOs.Student; // Student DTO'larını kullanmak için eklenmiş
using Core.DataAccess; // Core katmanındaki veri erişim arayüzlerini kullanmak için
using School.Business.Abstract; // İş katmanındaki soyut sınıfları kullanmak için
using School.Business.Utils; // İş katmanında yardımcı sınıflar için
using School.DataAccess; // Veri erişim işlemleri için
using School.Entities; // Varlık sınıflarını kullanmak için
using System; // Temel sistem sınıflarını kullanmak için
using System.Collections.Generic; // Koleksiyonlar için
using System.Linq; // LINQ sorguları için
using System.Text; // String işlemleri için
using System.Threading.Tasks; // Asenkron işlemler için

namespace School.Business.Concrete
{
    /// <summary>
    /// StudentManager sınıfı, öğrenci varlığı için CRUD işlemlerini gerçekleştiren iş katmanı sınıfıdır.
    /// CrudEntityManager sınıfını genişletir ve IStudentService arayüzünü uygular.
    /// </summary>
    public class StudentManager : CrudEntityManager<Student, StudentGetDto, StudentCreateDto, StudentUpdateDto>, IStudentService
    {
        private readonly IEntityRepository<Person> _personRepository; // Öğrenciye bağlı Person varlıklarını yönetmek için

        /// <summary>
        /// StudentManager constructor'ı. İlgili repository ve AutoMapper nesnelerini başlatır.
        /// </summary>
        /// <param name="unitOfWork">Birden fazla repository'yi yönetmek için kullanılan birim</param>
        /// <param name="mapper">Nesneleri dönüştürmek için AutoMapper</param>
        public StudentManager(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _personRepository = unitOfWork.GenerateRepository<Person>(); // Person repository'sini oluşturur
        }

        /// <summary>
        /// Student nesnesini DTO'ya dönüştürür ve Person bilgilerini ekler.
        /// </summary>
        /// <param name="input">Dönüştürülecek Student nesnesi</param>
        /// <returns>StudentGetDto nesnesi</returns>
        public override async Task<StudentGetDto> ConvertToDtoForGetAsync(Student input)
        {
            var person = await _personRepository.GetAsync(x => x.Id == input.PersonId); // Öğrenciye bağlı Person nesnesini alır

            if (person == null)
            {
                // Eğer Person bulunamazsa yalnızca Student nesnesini DTO'ya dönüştürür
                return Mapper.Map<Student, StudentGetDto>(input);
            }

            var studentGetDto = Mapper.Map<Student, StudentGetDto>(input); // Student'i DTO'ya dönüştürür
            return Mapper.Map(person, studentGetDto); // Person bilgilerini DTO'ya ekler
        }

        /// <summary>
        /// Yeni bir öğrenci ekler ve DTO olarak geri döner.
        /// </summary>
        /// <param name="input">Eklenecek öğrenci bilgileri</param>
        /// <returns>Eklenen öğrenci bilgilerini içeren DTO</returns>
        public override async Task<StudentGetDto> AddAsync(StudentCreateDto input)
        {
            var person = Mapper.Map<StudentCreateDto, Person>(input); // Öğrenciden Person nesnesi oluşturur
            var student = Mapper.Map<StudentCreateDto, Student>(input); // Öğrenciden Student nesnesi oluşturur

            await UnitOfWork.BeginTransactionAsync(); // Veritabanı işlemleri için bir transaction başlatır

            try
            {
                await _personRepository.AddAsync(person); // Person nesnesini ekler
                student.PersonId = person.Id; // Student nesnesine PersonId atanır

                await BaseEntityRepository.AddAsync(student); // Student nesnesini ekler

                var studentGetDto = Mapper.Map<StudentCreateDto, StudentGetDto>(input); // DTO'ya dönüştürür
                studentGetDto.Id = student.Id; // Öğrenci ID'sini ekler

                await UnitOfWork.CommitTransactionAsync(); // İşlemleri tamamlar

                return studentGetDto; // DTO olarak geri döner
            }
            catch (Exception ex)
            {
                await UnitOfWork.RollbaskTransactionAsync(); // Hata durumunda işlemleri geri alır
                throw new Exception(ex.Message); // Hatayı fırlatır
            }
        }

        /// <summary>
        /// Var olan bir öğrenciyi günceller.
        /// </summary>
        /// <param name="id">Güncellenecek öğrenci ID'si</param>
        /// <param name="input">Güncelleme bilgileri</param>
        /// <returns>Güncellenen öğrenci bilgilerini içeren DTO</returns>
        public override async Task<StudentGetDto> UpdateAsync(Guid id, StudentUpdateDto input)
        {
            var student = await BaseEntityRepository.GetAsync(x => x.Id == id); // Öğrenci nesnesini alır

            if (student == null)
            {
                return null; // Eğer öğrenci bulunamazsa null döner
            }

            var person = await _personRepository.GetAsync(x => x.Id == student.PersonId); // İlgili Person nesnesini alır

            if (person == null)
            {
                return null; // Eğer Person bulunamazsa null döner
            }

            person = Mapper.Map(input, person); // Güncelleme bilgilerini Person nesnesine uygular

            await _personRepository.UpdateAsync(person); // Person nesnesini günceller

            return await GetByIdAsync(id); // Güncellenmiş öğrenci bilgilerini döner
        }
    }
}
